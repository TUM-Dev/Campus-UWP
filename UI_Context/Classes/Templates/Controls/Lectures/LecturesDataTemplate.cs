using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Shared.Classes;
using Shared.Classes.Collections;
using Storage.Classes.Models.TumOnline;

namespace UI_Context.Classes.Templates.Controls.Lectures
{
    public class LecturesDataTemplate: AbstractDataTemplate, IComparable
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        private static readonly Regex SEMESTER_REGEX = new Regex(@"(\d\d)([W|S])");

        public readonly string HEADER;
        public readonly CustomObservableCollection<Lecture> LECTURES_GROUP;
        public bool expanded;

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--
        public LecturesDataTemplate(List<Lecture> lecturesList)
        {
            // There always has to be one grade:
            HEADER = lecturesList[0].SemesterId;
            LECTURES_GROUP = new CustomObservableCollection<Lecture>(true);
            LECTURES_GROUP.AddRange(lecturesList);
        }

        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public int CompareTo(object obj)
        {
            if (obj is LecturesDataTemplate other)
            {
                Match matchOther = SEMESTER_REGEX.Match(other.HEADER);
                if (matchOther is null || !matchOther.Success)
                {
                    return 1;
                }
                Match match = SEMESTER_REGEX.Match(HEADER);
                if (match is null || !match.Success)
                {
                    return -1;
                }

                if (other.HEADER == HEADER)
                {
                    return 0;
                }

                int yearOther = int.Parse(matchOther.Groups[1].Value);
                int year = int.Parse(match.Groups[1].Value);

                if (yearOther == year)
                {
                    if (matchOther.Groups[2].Value == "W")
                    {
                        return 1;
                    }
                    return -1;
                }
                return yearOther - year;
            }
            return 1;
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
