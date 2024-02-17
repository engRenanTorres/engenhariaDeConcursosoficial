using Microsoft.AspNetCore.Http;

namespace Apllication.Interfaces;

public interface IPhotosAcessor
{
  Task<PhotoUploadResult> AddPhoto(IFormFile file);
}

public class PhotoUploadResult { }
