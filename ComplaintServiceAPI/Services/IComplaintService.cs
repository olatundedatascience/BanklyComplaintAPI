using System;
using System.Collections;
using System.Collections.Generic;
using ComplaintServiceAPI.ComplaintServiceAPIContext;
using Microsoft.EntityFrameworkCore;


namespace ComplaintServiceAPI.Services
{
    public interface IComplaintService
    {
        bool CreateNewComplaint(Complaint complaint);
        bool CreateMultipleCoomplaint(IEnumerable<Complaint> complaints, out List<Complaint> FailedComplaints);

        bool DeleteComplaint(long Id);
        bool updateComplaint(long Id, Complaint complaint);
        bool updateStatus(long Id, bool status);

        Complaint GetComplaintById(long Id);
        IEnumerable<Complaint> GetAll();
        IEnumerable<Complaint> GetAll(Func<Complaint, bool> predicate);
    }


    public class ComplaintService : IComplaintService
    {
        private IBaseRespository _repo;
        public ComplaintService(IBaseRespository repo)
        {
            this._repo = repo;
        }
        public bool CreateNewComplaint(Complaint complaint)
        {
            try
            {
                var result = _repo.AddComplaint(complaint).Result;
                return result;
            }
            catch (Exception es)
            {
                throw es;
            }
            
        }

        public bool CreateMultipleCoomplaint(IEnumerable<Complaint> complaints, out List<Complaint> FailedComplaints)
        {
            try
            {
                var result = _repo.AddComplaintRange(complaints, out var failedComplaint).Result;
                FailedComplaints = failedComplaint;
                return result;
            }
            catch (Exception es)
            {
                throw es;
            }
        }

        public bool DeleteComplaint(long Id)
        {
            try
            {
                var result = _repo.Delete(Id).Result;

                return result;
            }
            catch (Exception es)
            {
                throw es;
            }
        }

        public bool updateComplaint(long Id, Complaint complaint)
        {
            try
            {
                var result = _repo.update(Id, complaint).Result;

                return result;
            }
            catch (Exception es)
            {
                throw es;
            }
            
        }

        public bool updateStatus(long Id, bool status)
        {
            try
            {
                var result = _repo.updateStatus(Id, status).Result;

                return result;
            }
            catch (Exception es)
            {
                throw es;
            }
        }

        public Complaint GetComplaintById(long Id)
        {
            try
            {
                var result = _repo.GetComplaint(Id).Result;

                return result;
            }
            catch (Exception es)
            {
                throw es;
            }
        }

        public IEnumerable<Complaint> GetAll()
        {
            try
            {
                var result = _repo.GetAllComplaint().Result;

                return result;
            }
            catch (Exception es)
            {
                throw es;
            }
        }

        public IEnumerable<Complaint> GetAll(Func<Complaint, bool> predicate)
        {
            try
            {
                var result = _repo.GetAllComplaint(predicate).Result;

                return result;
            }
            catch (Exception es)
            {
                throw es;
            }
        }
    }
}