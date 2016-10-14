using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Services
{
    public interface IThumbnailService
    {
        Image CreateThumbnailImage(int width, int height, Image image, bool center);
        Image CreateThumbnailImage(int size, Image image, bool center);

        Image GetImageFromBytes(byte[] imageBytes);

        byte[] GetImageBytes(Image image, ImageFormat format);
    }
}
