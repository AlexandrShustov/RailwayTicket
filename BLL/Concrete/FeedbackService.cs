using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;

namespace BLL.Concrete
{
    public class FeedbackService : IFeedbackService
    {
        private IUnitOfWork _unitOfWork;

        public FeedbackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Feedback> GetAll()
        {
            return _unitOfWork.FeedbackRepository.GetAll();
        }

        public Task CreateFeedback(Feedback feedback)
        {
            Guard.ArgumentNotNull(feedback, nameof(feedback) + " should not be null.");

            _unitOfWork.FeedbackRepository.Add(feedback);

            return _unitOfWork.SaveChangesAsync();
        }
    }
}