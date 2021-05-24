using System.Collections.Generic;

namespace foodisgood.Models
{
    public class ReviewModel
    {
        public IEnumerable<Rewiew> rewiews { get; set; }

        public string userId { get; set; }

        public string PersonLastname { get; set; }

        public string PersonFirstname { get; set; }

        public string StarsAverage { get; set; }
    }
}