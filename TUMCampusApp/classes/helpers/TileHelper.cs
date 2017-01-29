using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace TUMCampusApp.Classes.Helpers
{
    class TileHelper
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--

        #endregion
        //--------------------------------------------------------Construktor:----------------------------------------------------------------\\
        #region --Construktoren--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        public static async void PinTileAsync(string name, string text, string args, string logo)
        {
            SecondaryTile tile = new SecondaryTile(Const.TILE_ID_CANTEEN)
            {
                DisplayName = name,
                Arguments = args
            };
            tile.VisualElements.Square150x150Logo = Const.Square150x150Logo;
            tile.VisualElements.Wide310x150Logo = Const.Wide310x150Logo;
            tile.VisualElements.Square310x310Logo = Const.Square310x310Logo;
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
        private static TileContent generateTileContent(string text, string avatarLogoSource)
        {
            return new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = GenerateTileBindingMedium(text, avatarLogoSource),
                    TileWide = GenerateTileBindingWide(text, avatarLogoSource),
                    TileLarge = GenerateTileBindingLarge(text, avatarLogoSource)
                }
            };
        }

        private static TileBinding GenerateTileBindingMedium(string text, string avatarLogoSource)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    PeekImage = new TilePeekImage()
                    {
                        Source = avatarLogoSource,
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

        private static TileBinding GenerateTileBindingWide(string text, string avatarLogoSource)
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
                                    Source = avatarLogoSource,
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

        private static TileBinding GenerateTileBindingLarge(string text, string avatarLogoSource)
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
                                    Source = avatarLogoSource,
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
