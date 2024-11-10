using CVGS.Entities;

namespace CVGS.Service
{
    public class ReviewService
    {
        public List<Review> GetReviewForGame(CvgsDbContext context,int gameId, int start,int count)
        {
            List<Review> reviews = context.Review.Where(r=>r.GameId == gameId).Skip(start).Take(count).ToList();

            return reviews;
        }
        
        public List<Review> GetReviewFromUser(CvgsDbContext context,string userId, int start,int count)
        {
            List<Review> reviews = context.Review.Skip(start).Take(count).Where(r => r.UserId == userId).ToList();

            return reviews;
        }
    }
}
