using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Services
{
    public class ThumbnailService: IThumbnailService
    {
        public Image CreateThumbnailImage(int width, int height, Image image, bool center)
        {
            // Create our new image
            Bitmap newImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                if (center && image.Width != image.Height)
                {
                    Rectangle srcRect = new Rectangle();

                    if (image.Width > image.Height)
                    {
                        srcRect.Width = image.Height;
                        srcRect.Height = image.Height;
                        srcRect.X = (image.Width - image.Height) / 2;
                        srcRect.Y = 0;
                    }
                    else
                    {
                        srcRect.Width = image.Width;
                        srcRect.Height = image.Width;
                        srcRect.Y = (image.Height - image.Width) / 2;
                        srcRect.X = 0;
                    }

                    g.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(image, 0, 0, width, height);
                }
            }

            return newImage;
        }

        public Image CreateThumbnailImage(int size, Image image, bool center)
        {
            return CreateThumbnailImage(size, size, image, center);
        }

        public Image GetImageFromBytes(byte[] imageBytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);

                    return image;
                }
            }
            catch
            {
                return null;
            }
        }

        public byte[] GetImageBytes(Image image, ImageFormat format)
        {
            byte[] imageBytes;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    imageBytes = ms.ToArray();

                    return imageBytes;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
