using System.Collections.Generic;

namespace ExamCalendarAssigner.Data.Model
{
    public class DersModel
    {
        public string kod { get; set; }
        public string baslik { get; set; }
        public SinavModel sinav { get; set; }
        public List<ProgramModel> program { get; set; }
        public bool Atandi { get; set; }
    }
}