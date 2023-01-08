using AutoMapper;
using FileAppTask.Data;
using FileAppTask.Data.Enums;
using FileAppTask.Models.Models;
using File = FileAppTask.Data.File;
using FileShare = FileAppTask.Data.FileShare;

namespace FileAppTask.Api.Mapping
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<File, FileDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FileId))
               .ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => ((FileType)src.FileType).ToString().ToLower()))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.UploadDate.ToString("g")))
               .ForMember(dest => dest.NumberOfDownloads, opt => opt.MapFrom(src => src.DownloadCount));


            CreateMap<File, FileDownloadDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FileId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path));

            CreateMap<FileShare, FileShareDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FileShareId.ToString()))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.File.Name))
               .ForMember(dest => dest.DownloadAvailableUntil, opt => opt.MapFrom(src => src.DownloadAvailableUntil.ToString("g")))
               .ForMember(dest => dest.DownloadLink, opt => opt.Ignore())
               .AfterMap<DownloadLinkAction>();

        }
    }

    public class DownloadLinkAction : IMappingAction<FileShare, FileShareDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DownloadLinkAction(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Process(FileShare source, FileShareDto destination, ResolutionContext context)
        {
            var urlReferer = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
            var downloadLink = $"{urlReferer}downloadfile/{source.FileShareId}";
            destination.DownloadLink = downloadLink;
        }
    }
}
