namespace BookShop
{
    using System;
    using System.Linq;
    using Data;
    using Models;
    using System.Globalization;
    using System.Text;
   

    public class StartUp
    {
        public static void Main()
        {
            using (BookShopContext context = new BookShopContext())
            {
                //var input = int.Parse(Console.ReadLine());
                //IncreasePrices(context);

                Console.WriteLine(RemoveBooks(context)); // <--- Put your method there...
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            string ageRestriction = "";

            if (AgeRestriction.Minor.ToString().ToLower().Equals(command.ToLower()))
            {
                ageRestriction = "minor";
            }
            else if (AgeRestriction.Teen.ToString().ToLower().Equals(command.ToLower()))
            {
                ageRestriction = "teen";
            }
            else if (AgeRestriction.Adult.ToString().ToLower().Equals(command.ToLower()))
            {
                ageRestriction = "adult";
            }

            var books = context.Books
                .Where(b => b.AgeRestriction.ToString().ToLower() == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(b => b.Copies <= 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, goldenBooks);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksTitleAndPrice = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, booksTitleAndPrice);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                    .Where(b => b.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var inputDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b =>
                    b.ReleaseDate.Value < inputDate)
                .OrderByDescending(b => b.ReleaseDate.Value)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                .ToArray();


            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName + " " + a.LastName)
                .Select(a => a.FirstName + " " + a.LastName)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksAndAuthors = context.Books
                .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(ab => ab.Title + " " + $"({ab.Author.FirstName + " " + ab.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, booksAndAuthors);
        }

        public static int CountBooks(BookShopContext context, int input)
        {
            var booksCounts = context
                .Books
                .Count(b => b.Title.Length > input);

            return booksCounts;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            context
                .Authors
                .Select(b => new
                {
                    AuthorFullName = $"{b.FirstName} {b.LastName}",
                    BooksCopies = b.Books
                        .Select(c => c.Copies)
                        .Sum()
                })
                .OrderByDescending(b => b.BooksCopies)
                .ToList()
                .ForEach(a => sb.AppendLine($"{a.AuthorFullName} - {a.BooksCopies}"));

            return sb.ToString();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
                })
                .OrderByDescending(b => b.TotalProfit)
                .ThenBy(b => b.Name)
                .ToList()
                .ForEach(c => sb.AppendLine($"{c.Name} ${c.TotalProfit:f2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.Name,
                    Books = x.CategoryBooks.Select(s => new
                        {
                            s.Book.Title,
                            s.Book.ReleaseDate
                        })
                        .OrderByDescending(r => r.ReleaseDate)
                        .Take(3)
                        .ToArray()
                })
                .ToArray();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList()
                .ForEach(p => p.Price += 5);

            context.SaveChanges();

        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToDelete = context
                .Books
                .Where(b => b.Copies < 4200)
                .ToList();

            var countOfBooks = booksToDelete.Count;

            context.Books.RemoveRange(booksToDelete);

            context.SaveChanges();

            return countOfBooks;
        }
    }
}
