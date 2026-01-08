using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;


namespace BusinessLogicLayer.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _repository;

        public CategoryService()
        {
            _repository = new CategoryRepository();
        }

        public void AddCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new Exception("Kateqoriya adı boş ola bilməz!");

            if (category.Name.Length > 30)
                throw new Exception("Kateqoriya adı 30 simvoldan çox ola bilməz!");

            if (!string.IsNullOrWhiteSpace(category.Description) && category.Description.Length > 50)
                throw new Exception("Təsvir 50 simvoldan çox ola bilməz!");

            _repository.Add(category);
        }

        public Category GetCategoryById(int id)
        {
            if (id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            return _repository.GetById(id);
        }


        public List<Category> GetAllCategories()
        {
            return _repository.GetAll();
        }

        public void UpdateCategory(Category category)
        {
            if (category.Id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            var existing = _repository.GetById(category.Id);
            if (existing == null)
                throw new Exception("Kateqoriya tapılmadı!");

            _repository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            if (id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            var existing = _repository.GetById(id);
            if (existing == null)
                throw new Exception("Kateqoriya tapılmadı!");

            _repository.Delete(id);
        }

        public List<Category> SearchCategories(string keyword)
        {
            return _repository.Search(keyword);
        }




    }
}
