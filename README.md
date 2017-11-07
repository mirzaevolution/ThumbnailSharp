# ThumbnailSharp
A specialized library to create an image thumbnail from various sources with better result and supports different image formats. A Thumbnail is something that's not very important sometimes, but for softwares/web apps that require a thumbnail to be uploaded to their databases to be used later becomes so vital. And if we just need to create a simple thumbnail, why do we need a big image composition library that holds unnecessary methods for us? Here, we introduce a simple but powerful library for creating a nice thumbnail either from local or internet (supports async/await) that produces better result than **Image.GetThumbnailImage Method** from **System.Drawing.dll**. You can consume the result of operation either as a array of bytes or stream.

Here are some samples:

**Original Image (Landscape)**

![landscape-image](https://raw.githubusercontent.com/mirzaevolution/ThumbnailSharp.Client/master/AllSamples/Samples/Local/landscape.jpg)

**Thumbnail (Jpeg)**

*size is set to 250*

![landscape-image-thumbnail](https://raw.githubusercontent.com/mirzaevolution/ThumbnailSharp.Client/master/AllSamples/Samples/Local/landscape-thumbnail.jpg)






**Original Image (Portrait)**

![portrait-image](https://raw.githubusercontent.com/mirzaevolution/ThumbnailSharp.Client/master/AllSamples/Samples/Local/portrait.jpg)

**Thumbnail (Jpeg)**

*size is set to 250*

![portrait-image-thumbnail](https://raw.githubusercontent.com/mirzaevolution/ThumbnailSharp.Client/master/AllSamples/Samples/Local/portrait-thumbnail.jpg)





**Original Image (Square/Proportional)**

![square-image](https://raw.githubusercontent.com/mirzaevolution/ThumbnailSharp.Client/master/AllSamples/Samples/Local/proportional.jpg)

**Thumbnail (Jpeg)**

*size is set to 250*

![square-image](https://raw.githubusercontent.com/mirzaevolution/ThumbnailSharp.Client/master/AllSamples/Samples/Local/proportional-thumbnail.jpg)






How does it work?
Simple, you just need to pass thumbnail size (aspect ratio will be reserved), image source, and image format.

**Get thumbnail from internet source**

```csharp
byte[] resultBytes = await new ThumbnailCreator().CreateThumbnailBytesAsync(
	thumbnailSize: 250,
    urlAddress: new Uri("http://www.sample-image.com/image.jpg",UriKind.Absolute),
    imageFormat: Format.Jpeg
);
// or
Stream resultStream = await new ThumbnailCreator().CreateThumbnailStreamAsync(
	thumbnailSize: 250,
    urlAddress: new Uri("http://www.sample-image.com/image.png",UriKind.Absolute),
    imageFormat: Format.Png
);
```

**Get thumbnail from local source**

```csharp
byte[] resultBytes = new ThumbnailCreator().CreateThumbnailBytes(
	thumbnailSize: 300,
    imageFileLocation: @"C:\images\image.bmp",
    imageFormat: Format.Bmp
);
//or
Stream resultStream = new ThumbnailCreator().CreateThumbnailStream(
	thumbnailSize: 300,
    imageFileLocation: @"C:\images\image.bmp",
    imageFormat: Format.Bmp
);
```

**Get thumbnail from image stream**

```csharp
byte[] resultBytes = new ThumbnailCreator().CreateThumbnailBytes(
	thumbnailSize: 300,
    imageStream: new FileStream(@"C:\images\image.jpg",FileMode.Open,FileAccess.ReadWrite),
    imageFormat: Format.Jpeg
);
//or
Stream resultStream = new ThumbnailCreator().CreateThumbnailStream(
	thumbnailSize: 300,
  	imageStream: new FileStream(@"C:\images\image.jpg",FileMode.Open,FileAccess.ReadWrite),
    imageFormat: Format.Jpeg
);
```

**Get thumbnail from image bytes

```csharp
byte[] buffer = GetImageBytes(); //this is just fictitious method to get image data in bytes

byte[] resultBytes = new ThumbnailCreator().CreateThumbnailBytes(
	thumbnailSize: 300,
    imageBytes: buffer,
    imageFormat: Format.Gif
);
//or
Stream resultStream = new ThumbnailCreator().CreateThumbnailStream(
	thumbnailSize: 300,
    imageBytes: buffer,
    imageFormat: Format.Tiff
);
```



**Take a look our softwares that were built using this library:**

### [ThumbnailSharp.Clients](https://github.com/mirzaevolution/ThumbnailSharp.Client)


---

Ready to taste it? 

#### Install from [Nuget.Org](https://www.nuget.org/packages/ThumbnailSharp/1.0.0)

```
Install-Package ThumbnailSharp -Version 1.0.0
```

---



Created by: **[Mirza Ghulam Rasyid](https://twitter.com/mirzaevolution)**
