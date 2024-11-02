﻿public class Book
{
    public int id { get; set; }
    public string title { get; set; }
    public string author { get; set; }
    public string coverphoto { get; set; }
    public int year { get; set; }
    public string description { get; set; }
    public DateTime dateadded { get; set; }
    public bool isborrowed { get; set; }

    public int? UserId { get; set; }  
    public User User { get; set; }
}
