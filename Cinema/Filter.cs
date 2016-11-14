using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    class Filter
    {
        string[] filters;

        public Filter(params string[] filters)
        {
            this.filters = filters;
        }

        public bool Meets(Movie movie)
        {
            if (filters != null)
                foreach (var filter in filters)
                    if (!Meets(movie, filter))
                        return false;

            return true;
        }

        public IEnumerable<Movie> FilterOut(IEnumerable<Movie> movies)
        {
            var ret = new List<Movie>();

            foreach (var movie in movies)
            {
                bool cont = true;
                foreach (var filter in filters)
                    if (!Meets(movie, filter))
                    {
                        cont = false;
                        break;
                    }

                if (cont)
                    ret.Add(movie);
            }

            return ret;
        }

        public bool Meets(Movie movie, string filter)
        {
            return movie.Name.Contains(filter)
                    || movie.Director.Contains(filter)
                    || movie.Language.Contains(filter)
                    || movie.Tags.Contains(filter)
                    || movie.ReleaseDate.ToShortDateString().Contains(filter);
        }
    }
}
