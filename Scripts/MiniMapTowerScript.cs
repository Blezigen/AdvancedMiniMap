using System;
using System.Drawing.Imaging;
using AdvancedMinimap.Utilities;
using AdvancedMiniMap.Scripts.Entities;
using AdvancedMiniMap.Utilities;
using Ensage;
using Ensage.Common.Menu;
using Ensage.SDK.Menu;
using Ensage.SDK.Renderer;
using SharpDX;
using SharpDX.Direct2D1.Effects;
using Color = System.Drawing.Color;

namespace AdvancedMiniMap.Scripts
{
    public class MiniMapTowerScript : BaseScript
    {
        private Vector2 MiniMapSize => new Vector2(MiniMapSizeItem, MiniMapSizeItem);
        private Team PlayerTeam => ((Hero) Config.Main.Context.Value.Owner).Team;

        public MenuItem<bool> MiniMapIsRightPosition { get; set; }
        public MenuItem<bool> OnlyCanPushBuilding { get; set; }
        public MenuItem<Slider> MiniMapSizeItem { get; set; }

        public MiniMapTowerScript(Config config, string menuTitle, MenuFactory parentMenu = null) : base(config,
            menuTitle, parentMenu)
        {
            OnInGameUpdateEventEnable = true;
            MiniMapIsRightPosition = Menu.Item("MiniMap is right position", false);
            OnlyCanPushBuilding = Menu.Item("Only can push building", false);
            MiniMapSizeItem = Menu.Item<Slider>("MiniMap Size", new Slider(244, 109, 280));

            #region Fields

            T1BuildingRadiantTop = new MinimapBuilding(Database.BuildingName.goodguys_tower1_top);
            T1BuildingRadiantMiddle = new MinimapBuilding(Database.BuildingName.goodguys_tower1_mid);
            T1BuildingRadiantBottom = new MinimapBuilding(Database.BuildingName.goodguys_tower1_bot);
            T2BuildingRadiantTop = new MinimapBuilding(Database.BuildingName.goodguys_tower2_top);
            T2BuildingRadiantMiddle = new MinimapBuilding(Database.BuildingName.goodguys_tower2_mid);
            T2BuildingRadiantBottom = new MinimapBuilding(Database.BuildingName.goodguys_tower2_bot);
            T3BuildingRadiantTop = new MinimapBuilding(Database.BuildingName.goodguys_tower3_top);
            CasarmRadiantTopLeft = new MinimapBuilding(Database.BuildingName.goodguys_range_rax_top);
            CasarmRadiantTopRight = new MinimapBuilding(Database.BuildingName.goodguys_melee_rax_top);
            T3BuildingRadiantMiddle = new MinimapBuilding(Database.BuildingName.goodguys_tower3_mid);
            CasarmRadiantMiddleLeft = new MinimapBuilding(Database.BuildingName.goodguys_range_rax_mid);
            CasarmRadiantMiddleRight = new MinimapBuilding(Database.BuildingName.goodguys_melee_rax_mid);
            T3BuildingRadiantBottom = new MinimapBuilding(Database.BuildingName.goodguys_tower3_bot);
            CasarmRadiantBottomLeft = new MinimapBuilding(Database.BuildingName.goodguys_range_rax_bot);
            CasarmRadiantBottomRight = new MinimapBuilding(Database.BuildingName.goodguys_melee_rax_bot);
            T4BuildingRadiantTop = new MinimapBuilding(Database.BuildingName.goodguys_tower4, true);
            T4BuildingRadiantBottom = new MinimapBuilding(Database.BuildingName.goodguys_tower4, false);
            TroneRadiant = new MinimapBuilding(Database.BuildingName.goodguys_fort);

            T1BuildingDireTop = new MinimapBuilding(Database.BuildingName.badguys_tower1_top);
            T1BuildingDireMiddle = new MinimapBuilding(Database.BuildingName.badguys_tower1_mid);
            T1BuildingDireBottom = new MinimapBuilding(Database.BuildingName.badguys_tower1_bot);
            T2BuildingDireTop = new MinimapBuilding(Database.BuildingName.badguys_tower2_top);
            T2BuildingDireMiddle = new MinimapBuilding(Database.BuildingName.badguys_tower2_mid);
            T2BuildingDireBottom = new MinimapBuilding(Database.BuildingName.badguys_tower2_bot);
            T3BuildingDireTop = new MinimapBuilding(Database.BuildingName.badguys_tower3_top);
            CasarmDireTopLeft = new MinimapBuilding(Database.BuildingName.badguys_melee_rax_top);
            CasarmDireTopRight = new MinimapBuilding(Database.BuildingName.badguys_range_rax_top);
            T3BuildingDireMiddle = new MinimapBuilding(Database.BuildingName.badguys_tower3_mid);
            CasarmDireMiddleLeft = new MinimapBuilding(Database.BuildingName.badguys_melee_rax_mid);
            CasarmDireMiddleRight = new MinimapBuilding(Database.BuildingName.badguys_range_rax_mid);
            T3BuildingDireBottom = new MinimapBuilding(Database.BuildingName.badguys_tower3_bot);
            CasarmDireBottomLeft = new MinimapBuilding(Database.BuildingName.badguys_melee_rax_bot);
            CasarmDireBottomRight = new MinimapBuilding(Database.BuildingName.badguys_range_rax_bot);
            T4BuildingDireTop = new MinimapBuilding(Database.BuildingName.badguys_tower4, true);
            T4BuildingDireBottom = new MinimapBuilding(Database.BuildingName.badguys_tower4, false);
            TroneDire = new MinimapBuilding(Database.BuildingName.badguys_fort);

            #endregion
        }


        public override void LoadTexture(RenderMode renderMode, IRenderer render)
        {
            if (IsDx11)
            {
                D3D11TextureManagerUltra.LoadFromBitmap(Database.HudId.BuildingHealth.GetName(), Resources.Hud.Crop(Database.HudSprites[Database.HudId.BuildingHealth]));
                D3D11TextureManagerUltra.LoadFromBitmap(Database.HudId.BuildingHealthGreen.GetName(), Resources.Hud.Crop(Database.HudSprites[Database.HudId.BuildingHealthGreen]));
                D3D11TextureManagerUltra.LoadFromBitmap(Database.HudId.BuildingHealthRed.GetName(), Resources.Hud.Crop(Database.HudSprites[Database.HudId.BuildingHealthRed]));
            }
            else
            {
                render.TextureManager.LoadFromStream(Database.HudId.BuildingHealth.GetName(), Resources.Hud.Crop(Database.HudSprites[Database.HudId.BuildingHealth]).ToStream(ImageFormat.Png));
                render.TextureManager.LoadFromStream(Database.HudId.BuildingHealthGreen.GetName(), Resources.Hud.Crop(Database.HudSprites[Database.HudId.BuildingHealthGreen]).ToStream(ImageFormat.Png));
                render.TextureManager.LoadFromStream(Database.HudId.BuildingHealthRed.GetName(), Resources.Hud.Crop(Database.HudSprites[Database.HudId.BuildingHealthRed]).ToStream(ImageFormat.Png));
            }
        }

        protected override void OnInGameUpdate(EventArgs args)
        {
            T1BuildingRadiantTop.OnUpdate();
            T2BuildingRadiantTop.OnUpdate();
            T3BuildingRadiantTop.OnUpdate();
            CasarmRadiantTopLeft.OnUpdate();
            CasarmRadiantTopRight.OnUpdate();
            T1BuildingRadiantMiddle.OnUpdate();
            T2BuildingRadiantMiddle.OnUpdate();
            T3BuildingRadiantMiddle.OnUpdate();
            CasarmRadiantMiddleLeft.OnUpdate();
            CasarmRadiantMiddleRight.OnUpdate();
            T1BuildingRadiantBottom.OnUpdate();
            T2BuildingRadiantBottom.OnUpdate();
            T3BuildingRadiantBottom.OnUpdate();
            CasarmRadiantBottomLeft.OnUpdate();
            CasarmRadiantBottomRight.OnUpdate();
            T4BuildingRadiantTop.OnUpdate();
            T4BuildingRadiantBottom.OnUpdate();
            TroneRadiant.OnUpdate();

            T1BuildingDireTop.OnUpdate();
            T2BuildingDireTop.OnUpdate();
            T3BuildingDireTop.OnUpdate();
            CasarmDireTopLeft.OnUpdate();
            CasarmDireTopRight.OnUpdate();
            T1BuildingDireMiddle.OnUpdate();
            T2BuildingDireMiddle.OnUpdate();
            T3BuildingDireMiddle.OnUpdate();
            CasarmDireMiddleLeft.OnUpdate();
            CasarmDireMiddleRight.OnUpdate();
            T1BuildingDireBottom.OnUpdate();
            T2BuildingDireBottom.OnUpdate();
            T3BuildingDireBottom.OnUpdate();
            CasarmDireBottomLeft.OnUpdate();
            CasarmDireBottomRight.OnUpdate();
            T4BuildingDireTop.OnUpdate();
            T4BuildingDireBottom.OnUpdate();
            TroneDire.OnUpdate();
        }

        public void DrawTower(Vector2 pos, MinimapBuilding building, float iconSize = 12)
        {
            DrawBuilding(pos, building.Team, building.IsAlive, building.Health, building.MaxHealth, iconSize);
        }

        public void DrawCasarm(Vector2 pos, MinimapBuilding building, float iconSize = 10)
        {
            DrawBuilding(pos, building.Team, building.IsAlive, building.Health, building.MaxHealth, iconSize);
        }

        private void DrawTrone(Vector2 pos, MinimapBuilding building, float iconSize = 16)
        {
            DrawBuilding(pos, building.Team, building.IsAlive, building.Health, building.MaxHealth, iconSize);
        }

        public void DrawBuilding(Vector2 pos, Team team, bool isVisible, float health, float maxHealth, float iconSize)
        {

            Vector2 startPos;

            if (!MiniMapIsRightPosition)
            {
                startPos = IsDx11 ? new Vector2(0, Drawing.Height - MiniMapSize.Y) : new Vector2(0-1, Drawing.Height - MiniMapSize.Y - 1);
            }
            else
            { 
                startPos = IsDx11 ? new Vector2(Drawing.Width - MiniMapSize.X, Drawing.Height - MiniMapSize.Y) : new Vector2(Drawing.Width - MiniMapSize.X-1, Drawing.Height - MiniMapSize.Y-1);

            }

            iconSize = (100f / 280f) * iconSize;
            var kX = MiniMapSize.X / 100f;
            var kY = MiniMapSize.Y / 100f;
            pos = new Vector2(pos.X * kX, pos.Y *kY);
            pos += startPos;

            var sizeHealthBarIcon = new Vector2(iconSize * kX, iconSize * kY);

            var teamColorTexture = Database.HudId.BuildingHealth.GetName();
            if (PlayerTeam == Team.Radiant)
                teamColorTexture = team == Team.Dire ? Database.HudId.BuildingHealthRed.GetName() : Database.HudId.BuildingHealthGreen.GetName();
            else
                teamColorTexture = team != Team.Dire ? Database.HudId.BuildingHealthRed.GetName() : Database.HudId.BuildingHealthGreen.GetName();

            var sizeY = sizeHealthBarIcon.Y / maxHealth * health;

            if (isVisible)
            {
                Render.DrawTexture(Database.HudId.BuildingHealth.GetName(), new RectangleF(pos.X, pos.Y, sizeHealthBarIcon.X, sizeHealthBarIcon.Y));
                var rect = new RectangleF(pos.X, pos.Y + sizeHealthBarIcon.Y - sizeY, sizeHealthBarIcon.X,sizeY);

                Render.DrawTexture(teamColorTexture, rect);
                //                Render.DrawFilledRectangle(backgroundRectBorder, backgroundPlaceholderColor, backgroundPlaceholderColor, 1f);
                //                Render.DrawFilledRectangle(backgroundRect, Color.White, Color.White, 0f);
                //                Render.DrawFilledRectangle(rect, teamColor, teamColor, 0f);
            }
        }

        public override void OnDraw(object sender, EventArgs e)
        {
            if (!EnableAltHider && !AltIsDown) return;

            #region Dire

                DrawTower(new Vector2(17.857140f, 8.928572f), T1BuildingDireTop);
                if ((OnlyCanPushBuilding && !T1BuildingDireTop.IsAlive) || !OnlyCanPushBuilding)
                    DrawTower(new Vector2(47.857140f, 8.928572f), T2BuildingDireTop);

                if ((OnlyCanPushBuilding && !T3BuildingDireTop.IsAlive) || !OnlyCanPushBuilding)
                {
                    DrawCasarm(new Vector2(73.571430f, 9.285714f), CasarmDireTopRight);
                    DrawCasarm(new Vector2(73.571430f, 12.500000f), CasarmDireTopLeft);
                }
                if ((OnlyCanPushBuilding && !T2BuildingDireTop.IsAlive) || !OnlyCanPushBuilding)
                    DrawTower(new Vector2(70.714290f, 10.357140f), T3BuildingDireTop);

                DrawTower(new Vector2(51.428570f, 43.571430f), T1BuildingDireMiddle);

                if ((OnlyCanPushBuilding && !T1BuildingDireMiddle.IsAlive) || !OnlyCanPushBuilding)
                    DrawTower(new Vector2(64.285710f, 33.928570f), T2BuildingDireMiddle);

                if ((OnlyCanPushBuilding && !T3BuildingDireMiddle.IsAlive) || !OnlyCanPushBuilding)
                {
                    DrawCasarm(new Vector2(76.071430f, 21.071430f), CasarmDireMiddleRight);
                    DrawCasarm(new Vector2(78.571430f, 23.571430f), CasarmDireMiddleLeft);
                }

                if ((OnlyCanPushBuilding && !T2BuildingDireMiddle.IsAlive) || !OnlyCanPushBuilding)
                    DrawTower(new Vector2(75.357150f, 23.571430f), T3BuildingDireMiddle);

                DrawTower(new Vector2(88.571430f, 58.928570f), T1BuildingDireBottom);
                if ((OnlyCanPushBuilding && !T1BuildingDireBottom.IsAlive) || !OnlyCanPushBuilding)
                    DrawTower(new Vector2(88.214290f, 45.357140f), T2BuildingDireBottom);

                if ((OnlyCanPushBuilding && !T3BuildingDireBottom.IsAlive) || !OnlyCanPushBuilding)
                {
                    DrawCasarm(new Vector2(87.500000f, 26.428570f), CasarmDireBottomRight);
                    DrawCasarm(new Vector2(90.714290f, 26.428570f), CasarmDireBottomLeft);
                }

                if ((OnlyCanPushBuilding && !T2BuildingDireBottom.IsAlive) || !OnlyCanPushBuilding)
                    DrawTower(new Vector2(88.928570f, 28.214290f), T3BuildingDireBottom);

                if ((OnlyCanPushBuilding && !T4BuildingDireTop.IsAlive && !T4BuildingDireBottom.IsAlive ) || !OnlyCanPushBuilding)
                {
                    DrawTrone(new Vector2(82.500000f, 15.000000f), TroneDire);
                }

                if ((OnlyCanPushBuilding && (!T3BuildingDireTop.IsAlive || !T3BuildingDireBottom.IsAlive ||
                                                 !T3BuildingDireMiddle.IsAlive)) || !OnlyCanPushBuilding)
                {
                        DrawTower(new Vector2(79.285710f, 16.428570f), T4BuildingDireTop);
                        DrawTower(new Vector2(82.500000f, 19.642860f), T4BuildingDireBottom);
                }


            #endregion

            #region Radiant

            DrawTower(new Vector2(7.500000f, 36.071430f), T1BuildingRadiantTop);
            if (OnlyCanPushBuilding && !T1BuildingRadiantTop.IsAlive || !OnlyCanPushBuilding)
                DrawTower(new Vector2(8.214286f, 53.214290f), T2BuildingRadiantTop);

            if (OnlyCanPushBuilding && !T3BuildingRadiantTop.IsAlive || !OnlyCanPushBuilding)
            {
                DrawCasarm(new Vector2(4.285714f, 72.500000f), CasarmRadiantTopLeft);
                DrawCasarm(new Vector2(7.500000f, 72.500000f), CasarmRadiantTopRight);
            }

            if (OnlyCanPushBuilding && !T2BuildingRadiantTop.IsAlive || !OnlyCanPushBuilding)
                DrawTower(new Vector2(5.357143f, 69.642860f), T3BuildingRadiantTop);

            DrawTower(new Vector2(37.857140f, 56.785720f), T1BuildingRadiantMiddle);

            if (OnlyCanPushBuilding && !T1BuildingRadiantMiddle.IsAlive || !OnlyCanPushBuilding)
                DrawTower(new Vector2(25.357140f, 64.642860f), T2BuildingRadiantMiddle);

            if (OnlyCanPushBuilding && !T3BuildingRadiantMiddle.IsAlive || !OnlyCanPushBuilding)
            {
                DrawCasarm(new Vector2(15.714290f, 75.000000f), CasarmRadiantMiddleLeft);
                DrawCasarm(new Vector2(18.214290f, 77.500000f), CasarmRadiantMiddleRight);
            }

            if (OnlyCanPushBuilding && !T2BuildingRadiantMiddle.IsAlive || !OnlyCanPushBuilding)
                DrawTower(new Vector2(18.214290f, 74.285710f), T3BuildingRadiantMiddle);

            DrawTower(new Vector2(79.642860f, 87.142860f), T1BuildingRadiantBottom);
            if (OnlyCanPushBuilding && !T1BuildingRadiantBottom.IsAlive || !OnlyCanPushBuilding)
                DrawTower(new Vector2(47.142860f, 88.571430f), T2BuildingRadiantBottom);

            if (OnlyCanPushBuilding && !T3BuildingRadiantBottom.IsAlive || !OnlyCanPushBuilding)
            {
                DrawCasarm(new Vector2(20.714290f, 86.071430f), CasarmRadiantBottomLeft);
                DrawCasarm(new Vector2(20.714290f, 89.285710f), CasarmRadiantBottomRight);
            }

            if (OnlyCanPushBuilding && !T2BuildingRadiantBottom.IsAlive || !OnlyCanPushBuilding)
                DrawTower(new Vector2(22.500000f, 87.142860f), T3BuildingRadiantBottom);

            if (OnlyCanPushBuilding && !T4BuildingRadiantTop.IsAlive && !T4BuildingRadiantBottom.IsAlive ||
                !OnlyCanPushBuilding) DrawTrone(new Vector2(9.642858f, 81.428570f), TroneRadiant);

            if (OnlyCanPushBuilding && (!T3BuildingRadiantTop.IsAlive || !T3BuildingRadiantBottom.IsAlive ||
                                        !T3BuildingRadiantMiddle.IsAlive) || !OnlyCanPushBuilding)
            {
                DrawTower(new Vector2(11.071430f, 78.214290f), T4BuildingRadiantTop);
                DrawTower(new Vector2(14.285720f, 81.428570f), T4BuildingRadiantBottom);
            }




            #endregion
        }

        public override void OnActivate()
        {
        }

        public override void OnDeActivate()
        {
        }

        #region InitField

        private MinimapBuilding T1BuildingRadiantTop { get; }
        private MinimapBuilding T1BuildingRadiantMiddle { get; }
        private MinimapBuilding T1BuildingRadiantBottom { get; }

        private MinimapBuilding T2BuildingRadiantTop { get; }
        private MinimapBuilding T2BuildingRadiantMiddle { get; }
        private MinimapBuilding T2BuildingRadiantBottom { get; }

        private MinimapBuilding T3BuildingRadiantTop { get; }
        private MinimapBuilding CasarmRadiantTopLeft { get; }
        private MinimapBuilding CasarmRadiantTopRight { get; }

        private MinimapBuilding T3BuildingRadiantMiddle { get; }
        private MinimapBuilding CasarmRadiantMiddleLeft { get; }
        private MinimapBuilding CasarmRadiantMiddleRight { get; }

        private MinimapBuilding T3BuildingRadiantBottom { get; }
        private MinimapBuilding CasarmRadiantBottomLeft { get; }
        private MinimapBuilding CasarmRadiantBottomRight { get; }

        private MinimapBuilding T4BuildingRadiantTop { get; }
        private MinimapBuilding T4BuildingRadiantBottom { get; }
        private MinimapBuilding TroneRadiant { get; }


        private MinimapBuilding T1BuildingDireTop { get; }
        private MinimapBuilding T1BuildingDireMiddle { get; }
        private MinimapBuilding T1BuildingDireBottom { get; }

        private MinimapBuilding T2BuildingDireTop { get; }
        private MinimapBuilding T2BuildingDireMiddle { get; }
        private MinimapBuilding T2BuildingDireBottom { get; }

        private MinimapBuilding T3BuildingDireTop { get; }
        private MinimapBuilding CasarmDireTopLeft { get; }
        private MinimapBuilding CasarmDireTopRight { get; }

        private MinimapBuilding T3BuildingDireMiddle { get; }
        private MinimapBuilding CasarmDireMiddleLeft { get; }
        private MinimapBuilding CasarmDireMiddleRight { get; }

        private MinimapBuilding T3BuildingDireBottom { get; }
        private MinimapBuilding CasarmDireBottomLeft { get; }
        private MinimapBuilding CasarmDireBottomRight { get; }

        private MinimapBuilding T4BuildingDireTop { get; }
        private MinimapBuilding T4BuildingDireBottom { get; }
        private MinimapBuilding TroneDire { get; }

        #endregion
    }
}