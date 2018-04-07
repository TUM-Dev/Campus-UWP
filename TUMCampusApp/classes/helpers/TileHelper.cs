using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUMCampusAppAPI;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace TUMCampusApp.Classes.Helpers
{
    class TileHelper
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Pins a tile with the given logo, name, text to the start screen
        /// </summary>
        /// <param name="name">The name for the new tile</param>
        /// <param name="text">The text that should get written onto the tile</param>
        /// <param name="args">The load args for doing custom stuff if the app loads from clicking on this tile</param>
        /// <param name="logo">The tiles logo</param>
        public static async void PinTileAsync(string name, string text, string args, string logo)
        {
            SecondaryTile tile = new SecondaryTile(Consts.TILE_ID_CANTEEN)
            {
                DisplayName = name,
                Arguments = args
            };
            tile.VisualElements.Square150x150Logo = Consts.Square150x150Logo;
            tile.VisualElements.Wide310x150Logo = Consts.Wide310x150Logo;
            tile.VisualElements.Square310x310Logo = Consts.Square310x310Logo;
            tile.VisualElements.ShowNameOnSquare150x150Logo = true;
            tile.VisualElements.ShowNameOnSquare310x310Logo = true;
            tile.VisualElements.ShowNameOnWide310x150Logo = true;
            if (!await tile.RequestCreateAsync())
            {
                return;
            }
            TileContent content = generateTileContent(text, logo);
            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId).Update(new TileNotification(content.GetXml()));
            return;
        }

        #endregion

        #region --Misc Methods (Private)--
        /// <summary>
        /// Generates the content for the default canteen tile
        /// </summary>
        /// <param name="text">The text that should get written onto the tile</param>
        /// <param name="logoSource">The source string for the tile logo</param>
        /// <returns>Returns the created TileContent</returns>
        private static TileContent generateTileContent(string text, string logoSource)
        {
            return new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = GenerateTileBindingMedium(text, logoSource),
                    TileWide = GenerateTileBindingWide(text, logoSource),
                    TileLarge = GenerateTileBindingLarge(text, logoSource)
                }
            };
        }

        /// <summary>
        /// Generates the content for the medium canteen tile
        /// </summary>
        /// <param name="text">The text that should get written onto the tile</param>
        /// <param name="logoSource">The source string for the tile logo</param>
        /// <returns>Returns the created TileContent</returns>
        private static TileBinding GenerateTileBindingMedium(string text, string logoSource)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    PeekImage = new TilePeekImage()
                    {
                        Source = logoSource,
                        HintCrop = TilePeekImageCrop.Circle
                    },

                    TextStacking = TileTextStacking.Center,

                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = text,
                            HintAlign = AdaptiveTextAlign.Center,
                            HintStyle = AdaptiveTextStyle.Base
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Generates the content for the wide canteen tile
        /// </summary>
        /// <param name="text">The text that should get written onto the tile</param>
        /// <param name="logoSource">The source string for the tile logo</param>
        /// <returns>Returns the created TileContent</returns>
        private static TileBinding GenerateTileBindingWide(string text, string logoSource)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    Children =
            {
                new AdaptiveGroup()
                {
                    Children =
                    {
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 33,

                            Children =
                            {
                                new AdaptiveImage()
                                {
                                    Source = logoSource,
                                    HintCrop = AdaptiveImageCrop.Circle
                                }
                            }
                        },

                        new AdaptiveSubgroup()
                        {
                            HintTextStacking = AdaptiveSubgroupTextStacking.Center,

                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = text,
                                    HintStyle = AdaptiveTextStyle.Title
                                },
                            }
                        }
                    }
                }
            }
                }
            };
        }

        /// <summary>
        /// Generates the content for the large canteen tile
        /// </summary>
        /// <param name="text">The text that should get written onto the tile</param>
        /// <param name="logoSource">The source string for the tile logo</param>
        /// <returns>Returns the created TileContent</returns>
        private static TileBinding GenerateTileBindingLarge(string text, string logoSource)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    TextStacking = TileTextStacking.Center,

                    Children =
            {
                new AdaptiveGroup()
                {
                    Children =
                    {
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1
                        },

                        // We surround the image by two subgroups so that it doesn't take the full width
                        new AdaptiveSubgroup()
                        {
                            HintWeight = 2,
                            Children =
                            {
                                new AdaptiveImage()
                                {
                                    Source = logoSource,
                                    HintCrop = AdaptiveImageCrop.Circle
                                }
                            }
                        },

                        new AdaptiveSubgroup()
                        {
                            HintWeight = 1
                        }
                    }
                },

                new AdaptiveText()
                {
                    Text = text,
                    HintAlign = AdaptiveTextAlign.Center,
                    HintStyle = AdaptiveTextStyle.Title
                },
            }
                }
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
