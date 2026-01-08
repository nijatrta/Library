using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class MemberService
    {
        private readonly MemberRepository  _repository;

        public MemberService()
        {
            _repository = new MemberRepository();
        }

        public void AddMember(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.FullName))
                throw new Exception("Ad boş ola bilməz!");

            if (member.FullName.Length > 30)
                throw new Exception("Ad 30 simvoldan çox ola bilməz!");

            if (string.IsNullOrWhiteSpace(member.Email))
                throw new Exception("Email boş ola bilməz!");

            if (!IsValidEmail(member.Email))
                throw new Exception("Email formatı düzgün deyil!");

            if (member.Email.Length > 40)
                throw new Exception("Email 40 simvoldan çox ola bilməz!");

            if (string.IsNullOrWhiteSpace(member.PhoneNumber))
                throw new Exception("Telefon nömrəsi boş ola bilməz!");

            if (member.MembershipDate == default(DateTime))
                member.MembershipDate = DateTime.Now;

            _repository.Add(member);
        }

        public Member GetMemberById(int id)
        {
            if (id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            return _repository.GetById(id);
        }

        public List<Member> GetAllMembers()
        {
            return _repository.GetAll();
        }

        public void UpdateMember(Member member)
        {
            if (member.Id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            var existing = _repository.GetById(member.Id);
            if (existing == null)
                throw new Exception("Üzv tapılmadı!");

            _repository.Update(member);
        }

        public void DeleteMember(int id)
        {
            if (id <= 0)
                throw new Exception("Düzgün ID daxil edin!");

            var existing = _repository.GetById(id);
            if (existing == null)
                throw new Exception("Üzv tapılmadı!");

            _repository.Delete(id);
        }

        public List<Member> SearchMembers(string keyword)
        {
            return _repository.Search(keyword);
        }

        public List<Member> GetActiveMembers()
        {
            var allMembers = GetAllMembers();
            return allMembers.Where(m => m.IsActive).ToList();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern);
            }
            catch
            {
                return false;
            }
        }
    }
}
