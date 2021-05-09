using System.Collections.Generic;

namespace foodisgood.Models
{
    public class ReviewModel
    {
        public IEnumerable<Rewiew> rewiews { get; set; }

        public string userId { get; set; }
    }
}