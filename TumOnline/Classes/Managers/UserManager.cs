using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Logging.Classes;
using Shared.Classes;
using Shared.Classes.Image;
using Storage.Classes;
using Storage.Classes.Contexts;
using Storage.Classes.Models.TumOnline;
using TumOnline.Classes.Events;
using TumOnline.Classes.Exceptions;

namespace TumOnline.Classes.Managers
{
    public class UserManager: AbstractManager
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        public static readonly UserManager INSTANCE = new UserManager();
        private Task<User> updateTask;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public async Task<User> UpdateUserAsync(TumOnlineCredentials credentials, string obfuscatedId, bool force)
        {
            // Wait for the old update to finish first:
            if (!(updateTask is null) && !updateTask.IsCompleted)
            {
                try
                {
                    return await updateTask.ConfAwaitFalse();
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Awaiting for user task failed with:", e);
                    return null;
                }
            }

            updateTask = Task.Run(async () =>
            {
                if (!force && CacheDbContext.IsCacheEntryValid(TumOnlineService.LECTURES_PERSONAL.NAME))
                {
                    Logger.Info("No need to fetch user. Cache is still valid.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.Users.Include(ctx.GetIncludePaths(typeof(User))).FirstOrDefault();
                    }
                }
                User user = null;
                try
                {
                    user = await DownloadUserAsync(credentials, obfuscatedId, force);
                }
                catch (Exception e)
                {
                    InvokeOnRequestError(new RequestErrorEventArgs(e));
                    Logger.Error("Failed to request user with:", e);
                }
                if (!(user is null))
                {
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        foreach (User userOld in ctx.Users.Where(u => u.Id == user.Id).Include(ctx.GetIncludePaths(typeof(User))))
                        {
                            if (userOld.Groups.Count() > 0)
                            {
                                ctx.RemoveRange(userOld.Groups);
                                userOld.Groups.Clear();
                            }
                            ctx.Remove(userOld);
                        }
                        ctx.Add(user);
                    }
                    CacheDbContext.UpdateCacheEntry(TumOnlineService.LECTURES_PERSONAL.NAME, DateTime.Now.Add(TumOnlineService.LECTURES_PERSONAL.VALIDITY));
                }
                else
                {
                    Logger.Info("Loading user from DB.");
                    using (TumOnlineDbContext ctx = new TumOnlineDbContext())
                    {
                        return ctx.Users.Include(ctx.GetIncludePaths(typeof(User))).FirstOrDefault();
                    }
                }
                return user;
            });
            try
            {
                return await updateTask.ConfAwaitFalse();
            }
            catch (Exception e)
            {
                InvokeOnRequestError(new RequestErrorEventArgs(e));
                Logger.Error("Awaiting for user task failed with:", e);
            }
            return null;
        }

        #endregion

        #region --Misc Methods (Private)--
        private async Task<User> DownloadUserAsync(TumOnlineCredentials credentials, string obfuscatedId, bool force)
        {
            TumOnlineRequest request = new TumOnlineRequest(TumOnlineService.PERSON_DETAILS);
            request.AddQuery("pIdentNr", obfuscatedId);
            AccessManager.AddToken(request, credentials);
            XmlDocument doc = await request.RequestDocumentAsync(!force);
            return ParseUser(doc);
        }

        private static User ParseUser(XmlDocument doc)
        {
            if (!(doc is null))
            {
                if (!(doc.SelectSingleNode("/error") is null))
                {
                    throw new InvalidTumOnlineResponseException(null, "Failed to request user from TUM online.", doc.ToString());
                }
                XmlNode person = doc.SelectSingleNode("person");
                if (!int.TryParse(person.SelectSingleNode("nr").InnerText, out int nr))
                {
                    throw new MalformedXmlTumOnlineException(null, $"Missing 'nr' field when parsing an user.", person.ToString());
                }

                // Parse image:
                byte[] image = null;
                MediaType imageType = MediaType.None;
                XmlNode imageNode = person.SelectSingleNode("image_data");
                if (!(imageNode is null))
                {
                    bool isNull = false;
                    foreach (XmlAttribute att in imageNode.Attributes)
                    {
                        if (string.Equals(att.Name, "isnull"))
                        {
                            if (string.Equals(att.Value, "true"))
                            {
                                isNull = true;
                            }
                            break;
                        }
                    }

                    if (!isNull)
                    {
                        try
                        {
                            imageType = ImageUtils.ParseMediaType(imageNode.Attributes["contenttype"].Value);
                            if (imageType == MediaType.None)
                            {
                                Logger.Error($"Unknown media type for TUMonline user image '{imageNode.Attributes["contenttype"].Value}'.");
                            }
                            else
                            {

                                image = Convert.FromBase64String(imageNode.InnerText);

                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error("Failed to parse TUMonline user image node: " + image.ToString(), e);
                        }
                    }
                }

                return new User
                {
                    Id = nr,
                    FirstName = person.SelectSingleNode("vorname").InnerText,
                    LastName = person.SelectSingleNode("familienname").InnerText,
                    Email = person.SelectSingleNode("email").InnerText,
                    Gender = person.SelectSingleNode("geschlecht").InnerText,
                    ObfuscatedId = person.SelectSingleNode("obfuscated_id").InnerText,
                    Title = person.SelectSingleNode("titel").InnerText,
                    Image = image,
                    ImageType = imageType,
                    Groups = ParseUserGroups(person.SelectSingleNode("gruppen"))
                };
            }
            return null;
        }

        private static List<UserGroup> ParseUserGroups(XmlNode groupsNode)
        {
            if (!(groupsNode is null))
            {
                List<UserGroup> groups = new List<UserGroup>();
                foreach (XmlNode groupNode in groupsNode.SelectNodes("gruppe"))
                {
                    groups.Add(ParseUserGroup(groupNode));
                }
                return groups;
            }
            return new List<UserGroup>();
        }

        private static UserGroup ParseUserGroup(XmlNode groupNode)
        {
            return new UserGroup
            {
                Identifier = groupNode.SelectSingleNode("kennung").InnerText,
                Description = groupNode.SelectSingleNode("beschreibung").InnerText,
                Organization = groupNode.SelectSingleNode("org").InnerText,
                Title = groupNode.SelectSingleNode("titel").InnerText,
            };
        }

        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
