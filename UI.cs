using System;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;

namespace Library
{
    class Program
    {
        static BookService bookService = new BookService();
        static CategoryService categoryService = new CategoryService();
        static MemberService memberService = new MemberService();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║  KİTABXANA İDARƏETMƏ SİSTEMİ          ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.WriteLine("\n1. Kitablar");
                Console.WriteLine("2. Kateqoriyalar");
                Console.WriteLine("3. Üzvlər");
                Console.WriteLine("0. Çıxış");
                Console.Write("\nSeçim: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": BookMenu(); break;
                    case "2": CategoryMenu(); break;
                    case "3": MemberMenu(); break;
                    case "0": return;
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // KİTAB MENYUSU
        // ═══════════════════════════════════════════════════════════════
        static void BookMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("════════ KİTABLAR ════════");
                Console.WriteLine("1. Kitab əlavə et");
                Console.WriteLine("2. Bütün kitabları göstər");
                Console.WriteLine("3. Kitab axtar");
                Console.WriteLine("4. Kitab yenilə");
                Console.WriteLine("5. Kitab sil");
                Console.WriteLine("0. Geri");
                Console.Write("\nSeçim: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddBook(); break;
                        case "2": ShowAllBooks(); break;
                        case "3": SearchBooks(); break;
                        case "4": UpdateBook(); break;
                        case "5": DeleteBook(); break;
                        case "0": return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nXəta: {ex.Message}");
                    Console.ReadKey();
                }
            }
        }

        static void AddBook()
        {
            Console.Clear();
            Console.WriteLine("═══ YENİ KİTAB ═══\n");

            Console.Write("Kitab adı (max 30 simvol): ");
            var title = Console.ReadLine();

            Console.Write("Müəllif (max 25 simvol): ");
            var author = Console.ReadLine();

            Console.Write("ISBN (13 rəqəm): ");
            var isbn = Console.ReadLine();

            Console.Write("Nəşr ili: ");
            var year = int.Parse(Console.ReadLine());

            Console.Write("Kateqoriya ID: ");
            var catId = int.Parse(Console.ReadLine());

            Console.Write("Mövcuddur? (1-Bəli, 0-Xeyr): ");
            var available = Console.ReadLine() == "1";

            var book = new Book
            {
                Title = title,
                Author = author,
                ISBN = isbn,
                PublishedYear = year,
                CategoryId = catId,
                IsAvailable = available
            };

            bookService.AddBook(book);
            Console.WriteLine("\n✓ Kitab əlavə edildi!");
            Console.ReadKey();
        }

        static void ShowAllBooks()
        {
            Console.Clear();
            var books = bookService.GetAllBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("Kitab tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("═══ BÜTÜN KİTABLAR ═══\n");
            Console.WriteLine($"{"ID",-5} {"Ad",-30} {"Müəllif",-25} {"İl",-6}");
            Console.WriteLine(new string('-', 70));

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Id,-5} {book.Title,-30} {book.Author,-25} {book.PublishedYear,-6}");
            }

            Console.ReadKey();
        }

        static void SearchBooks()
        {
            Console.Clear();
            Console.Write("Axtarış (ad, müəllif və ya ISBN): ");
            var keyword = Console.ReadLine();

            var books = bookService.SearchBooks(keyword);

            if (books.Count == 0)
            {
                Console.WriteLine("\nNəticə tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n═══ AXTARIŞ NƏTİCƏSİ ═══\n");
            Console.WriteLine($"{"ID",-5} {"Ad",-30} {"Müəllif",-25}");
            Console.WriteLine(new string('-', 65));

            foreach (var book in books)
            {
                Console.WriteLine($"{book.Id,-5} {book.Title,-30} {book.Author,-25}");
            }

            Console.ReadKey();
        }

        static void UpdateBook()
        {
            Console.Clear();
            Console.Write("Yeniləmək istədiyiniz kitabın ID: ");
            var id = int.Parse(Console.ReadLine());

            var book = bookService.GetBookById(id);
            if (book == null)
            {
                Console.WriteLine("Kitab tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nCari: {book.Title} - {book.Author}");
            Console.WriteLine("\nYeni məlumatlar (boş buraxsanız dəyişməz):\n");

            Console.Write($"Yeni ad [{book.Title}]: ");
            var title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title)) book.Title = title;

            Console.Write($"Yeni müəllif [{book.Author}]: ");
            var author = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(author)) book.Author = author;

            bookService.UpdateBook(book);
            Console.WriteLine("\n✓ Kitab yeniləndi!");
            Console.ReadKey();
        }

        static void DeleteBook()
        {
            Console.Clear();
            Console.Write("Silmək istədiyiniz kitabın ID: ");
            var id = int.Parse(Console.ReadLine());

            bookService.DeleteBook(id);
            Console.WriteLine("\n✓ Kitab silindi!");
            Console.ReadKey();
        }

        // ═══════════════════════════════════════════════════════════════
        // KATEQORİYA MENYUSU
        // ═══════════════════════════════════════════════════════════════
        static void CategoryMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("════════ KATEQORİYALAR ════════");
                Console.WriteLine("1. Kateqoriya əlavə et");
                Console.WriteLine("2. Bütün kateqoriyaları göstər");
                Console.WriteLine("3. Kateqoriya axtar");
                Console.WriteLine("4. Kateqoriya yenilə");
                Console.WriteLine("5. Kateqoriya sil");
                Console.WriteLine("0. Geri");
                Console.Write("\nSeçim: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddCategory(); break;
                        case "2": ShowAllCategories(); break;
                        case "3": SearchCategories(); break;
                        case "4": UpdateCategory(); break;
                        case "5": DeleteCategory(); break;
                        case "0": return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nXəta: {ex.Message}");
                    Console.ReadKey();
                }
            }
        }

        static void AddCategory()
        {
            Console.Clear();
            Console.WriteLine("═══ YENİ KATEQORİYA ═══\n");

            Console.Write("Ad (max 30 simvol): ");
            var name = Console.ReadLine();

            Console.Write("Təsvir (max 50 simvol): ");
            var description = Console.ReadLine();

            var category = new Category
            {
                Name = name,
                Description = description
            };

            categoryService.AddCategory(category);
            Console.WriteLine("\n✓ Kateqoriya əlavə edildi!");
            Console.ReadKey();
        }

        static void ShowAllCategories()
        {
            Console.Clear();
            var categories = categoryService.GetAllCategories();

            if (categories.Count == 0)
            {
                Console.WriteLine("Kateqoriya tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("═══ BÜTÜN KATEQORİYALAR ═══\n");
            Console.WriteLine($"{"ID",-5} {"Ad",-30} {"Təsvir",-50}");
            Console.WriteLine(new string('-', 90));

            foreach (var cat in categories)
            {
                Console.WriteLine($"{cat.Id,-5} {cat.Name,-30} {cat.Description,-50}");
            }

            Console.ReadKey();
        }

        static void SearchCategories()
        {
            Console.Clear();
            Console.Write("Axtarış: ");
            var keyword = Console.ReadLine();

            var categories = categoryService.SearchCategories(keyword);

            if (categories.Count == 0)
            {
                Console.WriteLine("\nNəticə tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n═══ AXTARIŞ NƏTİCƏSİ ═══\n");
            foreach (var cat in categories)
            {
                Console.WriteLine($"ID: {cat.Id} | {cat.Name} - {cat.Description}");
            }

            Console.ReadKey();
        }

        static void UpdateCategory()
        {
            Console.Clear();
            Console.Write("Yeniləmək istədiyiniz kateqoriyanın ID: ");
            var id = int.Parse(Console.ReadLine());

            var category = categoryService.GetCategoryById(id);
            if (category == null)
            {
                Console.WriteLine("Kateqoriya tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nCari: {category.Name}");
            Console.WriteLine("\nYeni məlumatlar (boş buraxsanız dəyişməz):\n");

            Console.Write($"Yeni ad [{category.Name}]: ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) category.Name = name;

            Console.Write($"Yeni təsvir [{category.Description}]: ");
            var desc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(desc)) category.Description = desc;

            categoryService.UpdateCategory(category);
            Console.WriteLine("\n✓ Kateqoriya yeniləndi!");
            Console.ReadKey();
        }

        static void DeleteCategory()
        {
            Console.Clear();
            Console.Write("Silmək istədiyiniz kateqoriyanın ID: ");
            var id = int.Parse(Console.ReadLine());

            categoryService.DeleteCategory(id);
            Console.WriteLine("\n✓ Kateqoriya silindi!");
            Console.ReadKey();
        }

        // ═══════════════════════════════════════════════════════════════
        // ÜZV MENYUSU
        // ═══════════════════════════════════════════════════════════════
        static void MemberMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("════════ ÜZVLƏR ════════");
                Console.WriteLine("1. Üzv əlavə et");
                Console.WriteLine("2. Bütün üzvləri göstər");
                Console.WriteLine("3. Üzv axtar");
                Console.WriteLine("4. Üzv yenilə");
                Console.WriteLine("5. Üzv sil");
                Console.WriteLine("0. Geri");
                Console.Write("\nSeçim: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": AddMember(); break;
                        case "2": ShowAllMembers(); break;
                        case "3": SearchMembers(); break;
                        case "4": UpdateMember(); break;
                        case "5": DeleteMember(); break;
                        case "0": return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nXəta: {ex.Message}");
                    Console.ReadKey();
                }
            }
        }

        static void AddMember()
        {
            Console.Clear();
            Console.WriteLine("═══ YENİ ÜZV ═══\n");

            Console.Write("Ad Soyad (max 30 simvol): ");
            var name = Console.ReadLine();

            Console.Write("Email (max 40 simvol): ");
            var email = Console.ReadLine();

            Console.Write("Telefon (max 15 simvol): ");
            var phone = Console.ReadLine();

            Console.Write("Aktiv? (1-Bəli, 0-Xeyr): ");
            var active = Console.ReadLine() == "1";

            var member = new Member
            {
                FullName = name,
                Email = email,
                PhoneNumber = phone,
                MembershipDate = DateTime.Now,
                IsActive = active
            };

            memberService.AddMember(member);
            Console.WriteLine("\n✓ Üzv əlavə edildi!");
            Console.ReadKey();
        }

        static void ShowAllMembers()
        {
            Console.Clear();
            var members = memberService.GetAllMembers();

            if (members.Count == 0)
            {
                Console.WriteLine("Üzv tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("═══ BÜTÜN ÜZVLƏR ═══\n");
            Console.WriteLine($"{"ID",-5} {"Ad Soyad",-30} {"Email",-40}");
            Console.WriteLine(new string('-', 80));

            foreach (var member in members)
            {
                Console.WriteLine($"{member.Id,-5} {member.FullName,-30} {member.Email,-40}");
            }

            Console.ReadKey();
        }

        static void SearchMembers()
        {
            Console.Clear();
            Console.Write("Axtarış (ad və ya email): ");
            var keyword = Console.ReadLine();

            var members = memberService.SearchMembers(keyword);

            if (members.Count == 0)
            {
                Console.WriteLine("\nNəticə tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n═══ AXTARIŞ NƏTİCƏSİ ═══\n");
            foreach (var member in members)
            {
                Console.WriteLine($"ID: {member.Id} | {member.FullName} - {member.Email}");
            }

            Console.ReadKey();
        }

        static void UpdateMember()
        {
            Console.Clear();
            Console.Write("Yeniləmək istədiyiniz üzvün ID: ");
            var id = int.Parse(Console.ReadLine());

            var member = memberService.GetMemberById(id);
            if (member == null)
            {
                Console.WriteLine("Üzv tapılmadı!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nCari: {member.FullName} - {member.Email}");
            Console.WriteLine("\nYeni məlumatlar (boş buraxsanız dəyişməz):\n");

            Console.Write($"Yeni ad [{member.FullName}]: ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) member.FullName = name;

            Console.Write($"Yeni email [{member.Email}]: ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email)) member.Email = email;

            memberService.UpdateMember(member);
            Console.WriteLine("\n✓ Üzv yeniləndi!");
            Console.ReadKey();
        }

        static void DeleteMember()
        {
            Console.Clear();
            Console.Write("Silmək istədiyiniz üzvün ID: ");
            var id = int.Parse(Console.ReadLine());

            memberService.DeleteMember(id);
            Console.WriteLine("\n✓ Üzv silindi!");
            Console.ReadKey();
        }
    }
}
