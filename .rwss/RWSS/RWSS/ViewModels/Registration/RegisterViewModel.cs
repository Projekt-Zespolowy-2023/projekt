﻿using RWSS.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace RWSS.ViewModels.Registration
{
    public class RegisterViewModel
    {
        [Display(Name = "Pozycja")]
        [Required(ErrorMessage = "Pozycja jest wymagana")]
        public RoleCategory RoleCategory { get; set; }

        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        [Display(Name = "Numer PESEL")]
        [Required(ErrorMessage = "Numer PESEL jest wymagany")]
        public string PeselNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data urodzenia")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Adres email")]
        [Required(ErrorMessage = "Adres email jest wymagany")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Potwierdź hasło")]
        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła nie są te same")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Numer indeksu")]
        [Required(ErrorMessage = "Numer indeksu jest wymagany")]
        public string IndexNumber { get; set; }

        [Display(Name = "Rok studiów")]
        [Required(ErrorMessage = "Rok studiów jest wymagany")]
        public YearCategory YearCategory { get; set; }

        [Display(Name = "Semestr studiów")]
        [Required(ErrorMessage = "Semestr studiów jest wymagany")]
        public SemesterCategory SemesterCategory { get; set; }

        [Display(Name = "Kierunek studiów")]
        [Required(ErrorMessage = "Kierunek studiów jest wymagany")]
        public DegreeCourseCategory DegreeCourseCategory { get; set; }

        [Display(Name = "Stopień studiów")]
        [Required(ErrorMessage = "Stopień studiów jest wymagany")]
        public StudiesDegreeCategory StudiesDegreeCategory { get; set; }
    }
}
