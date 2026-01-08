using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {


        private const int ID_LENGTH = 5;
        private const int NAME_LENGTH = 30;
        private const int DESCRIPTION_LENGTH = 50;
        private const int TOTAL_LENGTH = 85;

        private readonly string _filePath;


        public CategoryRepository()
        {
            string projectRoot = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(projectRoot, "Data");

            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            _filePath = Path.Combine(dataFolder, "categories.txt");

            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();
        }



        public void Add(Category entity)
        {
            List<string> lines = new List<string>();
            string id = entity.Id.ToString().PadRight(ID_LENGTH).Substring(0, ID_LENGTH);
            string name = entity.Name.PadRight(NAME_LENGTH).Substring(0, NAME_LENGTH);
            string description = entity.Description.PadRight(DESCRIPTION_LENGTH).Substring(0, DESCRIPTION_LENGTH);
            string line = $"{id}{name}{description}";
            lines.Add(line);
            File.AppendAllLines(_filePath, lines);


        }

        public void Delete(int id)
        {
            if (!File.Exists(_filePath)) return;

            var lines = new List<string>(File.ReadAllLines(_filePath));
            lines.RemoveAll(line => !string.IsNullOrWhiteSpace(line) && ParseCategory(line).Id == id);
            File.WriteAllLines(_filePath, lines);

        }

        public List<Category> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<Category>();

            var lines = File.ReadAllLines(_filePath);
            var categories = new List<Category>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    categories.Add(ParseCategory(line));
                }
            }
            return categories;
        }

        public Category GetById(int id)
        {
            if (!File.Exists(_filePath))
                return null;
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    continue;
                var category = ParseCategory(line);
                if (category.Id == id)
                    return category;
            }
            return null;
        }

        public List<Category> Search(string keyword)
        {
            var categories = GetAll();
            if (string.IsNullOrWhiteSpace(keyword))
                return categories;
            keyword = keyword.ToLower();
            return categories.Where(c => c.Name.ToLower().Contains(keyword) || c.Description.ToLower().Contains(keyword)).ToList();
        }

        public void Update(Category entity)
        {
            if (!File.Exists(_filePath))
                return;
            var lines = new List<string>(File.ReadAllLines(_filePath));
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;
                var category = ParseCategory(lines[i]);
                if (category.Id == entity.Id)
                {
                    string id = entity.Id.ToString().PadRight(ID_LENGTH).Substring(0, ID_LENGTH);
                    string name = entity.Name.PadRight(NAME_LENGTH).Substring(0, NAME_LENGTH);
                    string description = entity.Description.PadRight(DESCRIPTION_LENGTH).Substring(0, DESCRIPTION_LENGTH);
                    lines[i] = $"{id}{name}{description}";
                    break;
                }
            }
            File.WriteAllLines(_filePath, lines);
        }


        private Category ParseCategory(string line)
        {
            if (line.Length < TOTAL_LENGTH)
                line = line.PadRight(TOTAL_LENGTH);

            int pos = 0;

            var category = new Category();

            category.Id = int.Parse(line.Substring(pos, ID_LENGTH).Trim());
            pos += ID_LENGTH;

            category.Name = line.Substring(pos, NAME_LENGTH).Trim();
            pos += NAME_LENGTH;

            category.Description = line.Substring(pos, DESCRIPTION_LENGTH).Trim();

            return category;
        }

        private string FormatCategoryToLine(Category category)
        {
            string id = category.Id.ToString().PadRight(ID_LENGTH).Substring(0, ID_LENGTH);
            string name = category.Name.PadRight(NAME_LENGTH).Substring(0, NAME_LENGTH);
            string description = category.Description.PadRight(DESCRIPTION_LENGTH).Substring(0, DESCRIPTION_LENGTH);
            return $"{id}{name}{description}";
        }


    }
}
