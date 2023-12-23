using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace MapEditor.Views.Elements;

public static class ImagesStorage
{
    private static Dictionary<string, BitmapImage>? images;
    public static BitmapImage GetImage(string imageName, string path, string type = ".png")
    {
        images ??= new Dictionary<string, BitmapImage>();
        string pathToImage = $"pack://application:,,,/Assets/{path}/{imageName}{type}";
        
        if (images.TryGetValue(pathToImage, out var img)) return img;
        
        BitmapImage image = new BitmapImage(new Uri(pathToImage));
        images.Add(pathToImage, image);
        return image;
    }
}