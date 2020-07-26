using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ComplaintAPI.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ComplaintAPI.Services
{
    public interface IBaseRespository
    {
        Task<bool> AddComplaint(Complaint complaint);
        Task<bool> AddComplaintRange(IEnumerable<Complaint> complatints, out List<Complaint> FailedComplaint);

        Task<bool> Delete(long Id);
       // Task<bool> Delete(long Id, Func<Complaint, bool> predicate);

        Task<Complaint> GetComplaint(long Id);
        Task<Complaint> GetComplaint(long Id, Func<Complaint, bool> predicate);

        Task<IEnumerable<Complaint>> GetAllComplaint();
        Task<IEnumerable<Complaint>> GetAllComplaint(Func<Complaint, bool> predicate);

        Task<bool> update(long Id, Complaint complaint);
    }

    public class BaseRepository : IBaseRespository
    {
        private ComplaintDbContext _db;
        private bool dbResult = false;
        public BaseRepository(ComplaintDbContext _db)
        {
            this._db = _db;
        }
        public Task<bool> AddComplaint(Complaint complaint)
        {
            Task<bool> addComplaintTask = new Task<bool>(() =>
            {
                 _db.Complaints.Add(complaint);
                 dbResult = _db.SaveChanges() > 0;

                 return dbResult;
            });
            addComplaintTask.Start();
            return addComplaintTask;
        }

        public Task<bool> AddComplaintRange(IEnumerable<Complaint> complatints, out List<Complaint> FailedComplaint)
        {
            var failedComplaints = new List<Complaint>();
            Task<bool> addComplaintRange = new Task<bool>(() =>
            {
                foreach (var currentComplaint in complatints)
                {
                    _db.Complaints.Add(currentComplaint);
                    dbResult = _db.SaveChanges() > 0;

                    if (!dbResult)
                    {
                        failedComplaints.Add(currentComplaint);
                    }
                    
                }

                return failedComplaints.Count < 1;
            });
            FailedComplaint = failedComplaints;
            addComplaintRange.Start();
            return addComplaintRange;
        }

        public Task<bool> Delete(long Id)
        {
            Task<bool> deleteTask = new Task<bool>(() =>
            {
                var complaint = _db.Complaints.SingleOrDefault(x => x.Id == Id);

                if (complaint != null)
                {
                    _db.Complaints.Remove(complaint);
                    dbResult = _db.SaveChanges() > 0;
                }
                else
                {
                    dbResult = false;
                }

                return dbResult;
            });
            deleteTask.Start();

            return deleteTask;
        }

        

        public Task<Complaint> GetComplaint(long Id)
        {
            Task<Complaint> complaintTask = new Task<Complaint>(() =>
            {
                var complaint = _db.Complaints.SingleOrDefault(x => x.Id == Id);
                return complaint;
            });
            
            complaintTask.Start();
            return complaintTask;
        }

        public Task<Complaint> GetComplaint(long Id, Func<Complaint, bool> predicate)
        {
            Task<Complaint> complaintTask = new Task<Complaint>(() =>
            {
                var complaint = _db.Complaints.Where(predicate).SingleOrDefault(x => x.Id == Id);
                return complaint;
            });
            
            complaintTask.Start();
            return complaintTask;
        }

        public Task<IEnumerable<Complaint>> GetAllComplaint()
        {
            Task<IEnumerable<Complaint>> complaintsTask = new Task<IEnumerable<Complaint>>(() =>
            {
                return _db.Complaints.AsEnumerable();
            });
            
            complaintsTask.Start();
            return complaintsTask;
        }

        public Task<IEnumerable<Complaint>> GetAllComplaint(Func<Complaint, bool> predicate)
        {
            Task<IEnumerable<Complaint>> complaintsTask = new Task<IEnumerable<Complaint>>(() =>
            {
                return _db.Complaints.Where(predicate);
            });
            
            complaintsTask.Start();
            return complaintsTask;
        }

        public Task<bool> update(long Id, Complaint complaint)
        {
            Task<bool> updateTask = new Task<bool>(() =>
            {
                var complaint = _db.Complaints.SingleOrDefault(x => x.Id == Id);
                _db.Complaints.Update(complaint);

                //_db.Entry<Complaint>(complaint).State = EntityState.Modified;
                dbResult = _db.SaveChanges() > 0;
                return dbResult;
            });
            
            updateTask.Start();
            return updateTask;
        }
    }
}