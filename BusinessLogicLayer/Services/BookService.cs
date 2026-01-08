using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class BookService
    {

        private readonly BookRepository _repository;


        public BookService()
        {
            _repository = new BookRepository();
        }

        

        public bool CheckIsbn(string isbn)
        {
            return IsbnHelper.IsValidIsbn(isbn);
        }

        public void AddBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))           
                throw new ArgumentException("Kitab adi bos ola bilemz");
            
            if (book.Title.Length > 30)
                throw new ArgumentException("Kitab adı 30 simvoldan çox ola bilməz!");
            
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new Exception("Müəllif adı boş ola bilməz!");

            if (book.Author.Length > 25)
                throw new Exception("Müəllif adı 25 simvoldan çox ola bilməz!");

            if (string.IsNullOrWhiteSpace(book.ISBN))
                throw new Exception("ISBN boş ola bilməz!");

            if (book.ISBN.Length != 13)
                throw new Exception("ISBN 13 simvoldan ibarət olmalıdır!");

            if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year)
                throw new Exception($"Nəşr ili 1000 ilə {DateTime.Now.Year} arasında olmalıdır!");

            if (book.CategoryId <= 0)
                throw new Exception("Kateqoriya ID düzgün deyil!");

            _repository.Add(book);
        }


        public Book GetBookById(int id)
        {
            if (id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            return _repository.GetById(id);
        }

        public List<Book> GetAllBooks()
        {
            return _repository.GetAll();
        }

        public void UpdateBook(Book book)
        {
            if (book.Id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            var existing = _repository.GetById(book.Id);
            if (existing == null)
                throw new Exception("Kitab tapılmadı!");

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new Exception("Kitab adı boş ola bilməz!");

            _repository.Update(book);
        }

        public void DeleteBook(int id)
        {
            if (id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            var existing = _repository.GetById(id);
            if (existing == null)
                throw new Exception("Kitab tapılmadı!");

            _repository.Delete(id);
        }

        public List<Book> SearchBooks(string keyword)
        {
            return _repository.Search(keyword);
        }

        public List<Book> GetBooksByCategory(int categoryId)
        {
            var allBooks = GetAllBooks();
            return allBooks.Where(b => b.CategoryId == categoryId).ToList();
        }

    }

}


        

        
