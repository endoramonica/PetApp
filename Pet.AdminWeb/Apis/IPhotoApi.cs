using Pet.Core.Application.DTOs;
using Refit;

namespace Pet.AdminWeb.Apis
{
    [Headers("Authorization: Bearer ")]
    public interface IPhotoApi
    {
        [Multipart]
        [Post("/api/photo/upload")]
        Task<ApiResult<PhotoUploadResult>> UploadPhotoAsync(
            [AliasAs("photo")] StreamPart photo,
            [AliasAs("folder")] string? folderPath = null
        );

        [Delete("/api/photo")]
        Task<ApiResult> DeletePhotoAsync([AliasAs("photoPath")] string photoPath);
    }
}
