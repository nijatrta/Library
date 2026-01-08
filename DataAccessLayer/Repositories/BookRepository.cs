using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    

    public class BookRepository : IRepository<Book>
    {

        private const int ID_LENGTH = 5;
        private const int TITLE_LENGTH = 30;
        private const int AUTHOR_LENGTH = 25;
        private const int ISBN_LENGTH = 13;
        private const int YEAR_LENGTH = 4;
        private const int CATEGORY_ID_LENGTH = 5;
        private const int AVAILABLE_LENGTH = 1;
        private const int TOTAL_LENGTH = 83;

        private readonly string _filePath;


        public void Delete(int id)
        {
            if (!File.Exists(_filePath)) return;

            var lines = new List<string>(File.ReadAllLines(_filePath));
            lines.RemoveAll(line => !string.IsNullOrWhiteSpace(line) && ParseBook(line).Id == id);
            File.WriteAllLines(_filePath, lines);
        }

        public void Add(Book entity)
        {
            var lines = File.Exists(_filePath)
                ? new List<string>(File.ReadAllLines(_filePath))
                : new List<string>();

            
            entity.Id = lines.Count > 0
                ? lines.Max(l => ParseBook(l).Id) + 1
                : 1;

            string line = FormatBookToLine(entity);
            lines.Add(line);

            File.WriteAllLines(_filePath, lines);
        }

        public List<Book> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Book>();

            var lines = File.ReadAllLines(_filePath);
            var books = new List<Book>();

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    books.Add(ParseBook(line));
            }

            return books;
        }

        public Book GetById(int id)
        {
            if (!File.Exists(_filePath)) return null;

            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var book = ParseBook(line);
                if (book.Id == id)
                    return book;
            }
            return null;
        }

        public List<Book> Search(string keyword)
        {
            var allBooks = GetAll();

            if (string.IsNullOrWhiteSpace(keyword))
                return allBooks;

            keyword = keyword.ToLower();

            return allBooks.Where(b =>
                b.Title.ToLower().Contains(keyword) ||
                b.Author.ToLower().Contains(keyword) ||
                b.ISBN.Contains(keyword)
            ).ToList();
        }

        public void Update(Book entity)
        {
            if (!File.Exists(_filePath)) return;

            var lines = new List<string>(File.ReadAllLines(_filePath));

            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var book = ParseBook(lines[i]);
                if (book.Id == entity.Id)
                {
                    lines[i] = FormatBookToLine(entity);
                    break;
                }
            }
        }


        private Book ParseBook(string line)
        {
            if (line.Length < TOTAL_LENGTH)
                line = line.PadRight(TOTAL_LENGTH);

            int position = 0;

            var book = new Book
            {
                Id = int.Parse(line.Substring(position, ID_LENGTH).Trim())
            };
            position += ID_LENGTH;

            book.Title = line.Substring(position, TITLE_LENGTH).Trim();
            position += TITLE_LENGTH;

            book.Author = line.Substring(position, AUTHOR_LENGTH).Trim();
            position += AUTHOR_LENGTH;

            book.ISBN = line.Substring(position, ISBN_LENGTH).Trim();
            position += ISBN_LENGTH;

            book.PublishedYear = int.Parse(line.Substring(position, YEAR_LENGTH).Trim());
            position += YEAR_LENGTH;

            book.CategoryId = int.Parse(line.Substring(position, CATEGORY_ID_LENGTH).Trim());
            position += CATEGORY_ID_LENGTH;

            book.IsAvailable = line.Substring(position, AVAILABLE_LENGTH) == "1";

            return book;
        }


        private string FormatBookToLine(Book book)
        {
            return book.Id.ToString().PadRight(ID_LENGTH) +
                   book.Title.PadRight(TITLE_LENGTH) +
                   book.Author.PadRight(AUTHOR_LENGTH) +
                   book.ISBN.PadRight(ISBN_LENGTH) +
                   book.PublishedYear.ToString().PadRight(YEAR_LENGTH) +
                   book.CategoryId.ToString().PadRight(CATEGORY_ID_LENGTH) +
                   (book.IsAvailable ? "1" : "0");
        }

    }
}
