using System.Web;
using IRunesModels;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunesApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TrackController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return BadRequestError("You are not logged in");
            }

            var albumId = request.QueryData["albumId"].ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View("Create");
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return BadRequestError("You are not logged in");
            }

            var albumId = request.QueryData["albumId"].ToString();

            var name = request.FormData["name"].ToString();
            var link = request.FormData["link"].ToString();
            var price = decimal.Parse(request.FormData["price"].ToString());

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(link) || price <= 0)
            {
                return this.View("/Tracks/Create");
            }

            try
            {
                AddTrackToDb(albumId, name, link, price);
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return new RedirectResult($"/Albums/Details?id={albumId}");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return BadRequestError("You are not logged in");
            }

            var albumId = request.QueryData["albumId"].ToString();
            var trackId = request.QueryData["trackId"].ToString();

            var track = this.Db.Tracks.FirstOrDefault(t => t.Id == trackId);
            var trackLink = HttpUtility.UrlDecode(track.Link);

            var trackData = new StringBuilder();

            trackData.Append($@"<iframe src=""{trackLink}""></iframe>");
            trackData.Append($@"<h2>Name: {track.Name}</h2></ br>");
            trackData.Append($@"<h2>Price: {track.Price:F2}&#36</h2></ br>");

            this.ViewBag["trackData"] = trackData.ToString();
            this.ViewBag["albumId"] = albumId;

            return this.View("Details");
        }

        private void AddTrackToDb(string albumId, string name, string link, decimal price)
        {
            var track = new Track
            {
                Name = name,
                Link = link,
                Price = price
            };

            this.Db.Tracks.Add(track);
            this.Db.SaveChanges();

            AddTrackAlbumToDb(albumId, track);
        }

        private void AddTrackAlbumToDb(string albumId, Track track)
        {
            var trackAlbum = new TrackAlbum()
            {
                TrackId = track.Id,
                AlbumId = albumId,
            };

            this.Db.TrackAlbums.Add(trackAlbum);
            this.Db.SaveChanges();
        }
    }
}
