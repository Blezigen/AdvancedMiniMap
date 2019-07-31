using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AdvancedMiniMap.Utilities
{
    public static class AdvanceUtility
    {
        public static Bitmap Crop(this Image image, Rectangle selection)
        {
            // Check if it is a bitmap:
            if (!(image is Bitmap bmp)) throw new ArgumentException("No valid bitmap");

            // Crop the image:
            var cropBmp = bmp.Clone(selection, bmp.PixelFormat);

            // Release the resources:
            image.Dispose();

            return cropBmp;
        }

        public static Stream ToStream(this Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        public static string GetName(this Database.BuildingName entityName)
        {
            var name = Enum.GetName(typeof(Database.BuildingName), entityName);
            return $@"npc_dota_{name}";
        }

        public static string GetName(this Database.HudId entityName)
        {
            var name = Enum.GetName(typeof(Database.HudId), entityName);
            return $@"hud_{name}";
        }
    }
}
