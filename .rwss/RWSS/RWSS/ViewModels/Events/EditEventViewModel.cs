using RWSS.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace RWSS.ViewModels.Events
{
    public class EditEventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PlaceOfEventCategory PlaceOfEventCategory { get; set; }
        public EventCategory EventCategory { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UpdateDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }
        public YearCategory YearCategory { get; set; }
    }
}
