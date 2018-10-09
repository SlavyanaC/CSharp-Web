namespace IRunesApp.Controllers
{
    using System;
    using System.Text;
    using System.Web;
    using System.Linq;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using IRunesModels;
    using SIS.WebServer.Results;

    public class AlbumController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            return this.View("Create");
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var name = request.FormData["name"].ToString();
            var cover = request.FormData["cover"].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(cover))
            {
                return new RedirectResult("/Albums/Create");
            }

            try
            {
                AddAlbumToDb(name, cover, request);

            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return new RedirectResult("/Albums/Create");
        }

        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var albums = this.Db.Albums;
            var listOfAlbums = string.Empty;
            if (albums.Any())
            {
                foreach (var album in albums)
                {
                    var albumHtml =
                        $@"<strong><li class""list-group-item""><a class=""list-group-item list-group-item-action text-primary"" href =""/Albums/Details?id={album.Id}"">{album.Name}</a></li></strong>";
                    listOfAlbums += albumHtml;
                }

                this.ViewBag["albumsList"] = listOfAlbums;
            }

            else
            {
                this.ViewBag["albumsList"] = "There are currently no albums.";
            }

            return this.View("All");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/Users/Login");
            }

            var albumId = request.QueryData["id"].ToString();
            var album = this.Db.Albums.FirstOrDefault(a => a.Id == albumId);
            var albumCover = HttpUtility.UrlDecode(album.Cover);

            var albumData = new StringBuilder();

            albumData.Append($@"<img src=""{albumCover}"" style=""height: 300px; width: 600px"" class=""img-thumbnail""></ br>");
            albumData.Append($@"<h2 class=""text-center"">Name: {album.Name}</h2></ br>");
            albumData.Append($@"<h2 class=""text-center"">Price: ${album.Price:F2}</h2></ br>");

            var tracksData = new StringBuilder();

            if (!album.TrackAlbums.Any())
            {
                tracksData.Append(@"<p class=""text-muted""><strong>No tracks in the album</strong></p>");
            }

            else
            {
                tracksData.Append("<ol>");
                foreach (var trackAlbum in album.TrackAlbums)
                {
                    tracksData.Append($@"<em><li><a class=""text-primary"" href=""/Tracks/Details?albumId={album.Id}&trackId={trackAlbum.Track.Id}"">{trackAlbum.Track.Name}</a></li><em>");
                }

                tracksData.Append("</ol>");
            }

            this.ViewBag["albumData"] = albumData.ToString();
            this.ViewBag["albumId"] = albumId;
            this.ViewBag["tracksData"] = tracksData.ToString();

            return this.View("Details");
        }

        private void AddAlbumToDb(string name, string cover, IHttpRequest request)
        {
            var album = new Album
            {
                Name = name,
                Cover = cover,
            };

            this.Db.Albums.Add(album);
            this.Db.SaveChanges();

            var username = this.GetUsername(request);

            var user = this.Db.Users.FirstOrDefault(u => u.Username == username);
            AddUserAlbumToDb(user, album);
        }

        private void AddUserAlbumToDb(User user, Album album)
        {
            var userAlbum = new UserAlbum
            {
                UserId = user.Id,
                AlbumId = album.Id,
            };

            this.Db.UserAlbums.Add(userAlbum);
            this.Db.SaveChanges();
        }
    }
}
