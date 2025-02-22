using System;
using System.Collections.Generic;

class Book
{
    public int BookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Book(int bookID, string title, string author)
    {
        BookID = bookID;
        Title = title;
        Author = author;
    }

    public void DisplayBookDetails()
    {
        Console.WriteLine($"Book ID: {BookID}, Title: {Title}, Author: {Author}");
    }

    public void DisplayBookDetails(bool showAvailability)
    {
        Console.WriteLine($"Book ID: {BookID}, Title: {Title}, Author: {Author}, Available: {IsAvailable}");
    }
}

class User
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public int BorrowedBookID { get; set; } = -1;

    public User(int userID, string name)
    {
        UserID = userID;
        Name = name;
    }

    public virtual void DisplayUserDetails()
    {
        Console.WriteLine($"User ID: {UserID}, Name: {Name}, Borrowed Book ID: {(BorrowedBookID == -1 ? "None" : BorrowedBookID.ToString())}");
    }
}

class PremiumUser : User
{
    public string MembershipLevel { get; set; }

    public PremiumUser(int userID, string name, string membershipLevel) : base(userID, name)
    {
        MembershipLevel = membershipLevel;
    }

    public override void DisplayUserDetails()
    {
        Console.WriteLine($"User ID: {UserID}, Name: {Name}, Membership Level: {MembershipLevel}, Borrowed Book ID: {(BorrowedBookID == -1 ? "None" : BorrowedBookID.ToString())}");
    }
}

class Library
{
    public List<Book> Books { get; set; } = new List<Book>();
    public List<User> Users { get; set; } = new List<User>();

    public void AddBook(Book book)
    {
        Books.Add(book);
        Console.WriteLine($"Book Added: [ID: {book.BookID}, Title: \"{book.Title}\", Author: \"{book.Author}\"]");
    }

    public void AddUser(User user)
    {
        Users.Add(user);
        Console.WriteLine($"User Added: [ID: {user.UserID}, Name: \"{user.Name}\"]");
    }

    public void BorrowBook(int userID, int bookID)
    {
        User user = Users.Find(u => u.UserID == userID);
        Book book = Books.Find(b => b.BookID == bookID);

        if (user != null && book != null && book.IsAvailable)
        {
            user.BorrowedBookID = bookID;
            book.IsAvailable = false;
            Console.WriteLine($"{user.Name} borrowed \"{book.Title}\".");
        }
        else
        {
            Console.WriteLine("Book is not available or user not found.");
        }
    }

    public void ReturnBook(int userID)
    {
        User user = Users.Find(u => u.UserID == userID);
        if (user != null && user.BorrowedBookID != -1)
        {
            Book book = Books.Find(b => b.BookID == user.BorrowedBookID);
            if (book != null)
            {
                book.IsAvailable = true;
                Console.WriteLine($"{user.Name} returned \"{book.Title}\".");
                user.BorrowedBookID = -1;
            }
        }
        else
        {
            Console.WriteLine("No borrowed book to return.");
        }
    }
}

class Program
{
    static void Main()
    {
        Library library = new Library();

        Book book1 = new Book(101, "C# Basics", "John Doe");
        Book book2 = new Book(102, "Advanced C#", "Jane Doe");
        
        library.AddBook(book1);
        library.AddBook(book2);

        User user1 = new User(1, "Alice");
        PremiumUser user2 = new PremiumUser(2, "Bob", "Gold");

        library.AddUser(user1);
        library.AddUser(user2);

        Console.WriteLine();
        book1.DisplayBookDetails(true);
        user1.DisplayUserDetails();

        Console.WriteLine();
        library.BorrowBook(1, 101);
        book1.DisplayBookDetails(true);

        Console.WriteLine();
        library.ReturnBook(1);
        book1.DisplayBookDetails(true);
    }
}
