using RWSS.Data.Enum;
using RWSS.Models;
using System.ComponentModel.DataAnnotations;

namespace RWSS.ViewModels.Events
{
    public class CreateEventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PlaceOfEventCategory PlaceOfEventCategory { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime EventDate { get; set; }
        public EventCategory EventCategory { get; set; }
        public string AppUserId { get; set; }
        public YearCategory YearCategory { get; set; }
    }
}
