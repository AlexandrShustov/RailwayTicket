using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface IFeedbackService
    {
        List<Feedback> GetAll();

        Task CreateFeedback(Feedback feedback);
    }
}