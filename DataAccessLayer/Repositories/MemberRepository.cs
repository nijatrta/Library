using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{

    public class MemberRepository : IRepository<Member>
    {

        private const int ID_LENGTH = 5;
        private const int FULLNAME_LENGTH = 30;
        private const int EMAIL_LENGTH = 40;
        private const int PHONE_LENGTH = 15;
        private const int DATE_LENGTH = 10;
        private const int ACTIVE_LENGTH = 1;
        private const int TOTAL_LENGTH = 101;

        private readonly string _filePath;


        public MemberRepository()
        {
            string projectRoot = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(projectRoot, "Data");
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);
            _filePath = Path.Combine(dataFolder, "members.txt");
            if (!File.Exists(_filePath))
                File.Create(_filePath).Close();

        }



        public void Add(Member entity)
        {
            List<string> lines;

            if (File.Exists(_filePath))
            {
                var fileLines = File.ReadAllLines(_filePath).ToList();
                lines = new List<string>(fileLines);
            }
            else
            {
                lines = new List<string>();
            }

            entity.Id = lines.Count > 0
                ? lines.Max(l => ParseMember(l).Id) + 1
                : 1;
            string line = FormatMemberToLine(entity);
            lines.Add(line);
            File.WriteAllLines(_filePath, lines);

        }

        public void Delete(int id)
        {
           if (!File.Exists(_filePath)) 
                return;
            var lines = new List<string>(File.ReadAllLines(_filePath));
            lines.RemoveAll(line => !string.IsNullOrWhiteSpace(line) && ParseMember(line).Id == id);
            File.WriteAllLines(_filePath, lines);
        }

        public List<Member> GetAll()
        {
            if (!File.Exists(_filePath)) 
                return new List<Member>();

            var lines = File.ReadAllLines(_filePath);
            var members = new List<Member>();

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var member = ParseMember(line);
                    members.Add(member);
                }
            }
            return members;
        }

        public Member GetById(int id)
        {
            if (!File.Exists(_filePath)) 
                return null;
            var lines = File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var member = ParseMember(line);
                if (member.Id == id)
                    return member;
            }

            return null;
        }

        public List<Member> Search(string keyword)
        {
            var allMembers = GetAll();

            if (string.IsNullOrWhiteSpace(keyword))
                return allMembers;
            keyword = keyword.ToLower();
            return allMembers.Where(m =>
                m.FullName.ToLower().Contains(keyword) ||
                m.Email.ToLower().Contains(keyword) ||
                m.PhoneNumber.ToLower().Contains(keyword)
            ).ToList();
        }

        public void Update(Member entity)
        {
            if (!File.Exists(_filePath)) 
                return;

            var lines = new List<string>(File.ReadAllLines(_filePath));
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;


                var member = ParseMember(lines[i]);
                if (member.Id == entity.Id)
                {
                    lines[i] = FormatMemberToLine(entity);
                    break;
                }
            }
            File.WriteAllLines(_filePath, lines);
        }


        private Member ParseMember(string line)
        {
            if (line.Length < TOTAL_LENGTH)
                line = line.PadRight(TOTAL_LENGTH);

            int pos = 0;

            var member = new Member();

            member.Id = int.Parse(line.Substring(pos, ID_LENGTH).Trim());
            pos += ID_LENGTH;

            member.FullName = line.Substring(pos, FULLNAME_LENGTH).Trim();
            pos += FULLNAME_LENGTH;

            member.Email = line.Substring(pos, EMAIL_LENGTH).Trim();
            pos += EMAIL_LENGTH;

            member.PhoneNumber = line.Substring(pos, PHONE_LENGTH).Trim();
            pos += PHONE_LENGTH;

            member.MembershipDate = DateTime.Parse(line.Substring(pos, DATE_LENGTH).Trim());
            pos += DATE_LENGTH;

            member.IsActive = line.Substring(pos, ACTIVE_LENGTH) == "1";

            return member;
        }

        private string FormatMemberToLine(Member member)
        {
            return member.Id.ToString().PadRight(ID_LENGTH) +
                   member.FullName.PadRight(FULLNAME_LENGTH) +
                   member.Email.PadRight(EMAIL_LENGTH) +
                   member.PhoneNumber.PadRight(PHONE_LENGTH) +
                   member.MembershipDate.ToString("yyyy-MM-dd") +
                   (member.IsActive ? "1" : "0");
        }

    }
}