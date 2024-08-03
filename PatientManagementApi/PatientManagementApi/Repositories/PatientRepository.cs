using PatientManagementApi.Extensions;
using System.Linq.Expressions;

namespace PatientManagementApi.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context)
        {
            
        }
        public async Task<PaginationResult<Patient>> SearchPatientAsync
            (PaginationRequest request,
            string? firstName,
            string? lastName,
            DateTime? dOB,
            string? phone,
            string? email,
            bool? isActive,
            Gender? gender)
        {
            var patientExpression=   createExpression(firstName, lastName, dOB, phone, email, isActive, gender);
            var query = _context.Patients.Where(patientExpression);   

            var totalCount = await query.CountAsync();

            var patients = await query.Skip(request.PageSize * request.PageIndex)
                                   .Take(request.PageSize)
                                   .ToListAsync();

            return new PaginationResult<Patient>(
                request.PageIndex,
                request.PageSize,
                totalCount,
                patients);
        }

        private Expression<Func<Patient, bool>> createExpression
            (string? firstName, string? lastName, DateTime? dOB, string ? phone, string ? email, bool? isActive, Gender? gender)
        {
            Expression<Func<Patient, bool>> patientExpression = patient => true;

            if (!string.IsNullOrEmpty(firstName))
            {
                patientExpression = patientExpression.And(p =>
                    EF.Functions.ILike(p.FirstName, $"%{firstName}%"));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                patientExpression = patientExpression.And(p => EF.Functions.ILike(p.LastName, $"%{lastName}%"));
            }

            if (dOB.HasValue)
            {
                var dateString = dOB.Value.ToString("yyyy-MM-dd");
                patientExpression = patientExpression.And(p => EF.Functions.ILike(p.DateOfBirth.ToString("yyyy-MM-dd"), $"%{dateString}%"));
            }


            if (!string.IsNullOrEmpty(phone))
            {
                patientExpression = patientExpression.And(p =>
                    p.ContactInfors.Any(c => c.Type == ContactType.Phone && c.Value == phone));
            }

            if (!string.IsNullOrEmpty(email))
            {
                patientExpression = patientExpression.And(p =>
                    p.ContactInfors.Any(c => c.Type == ContactType.Email && EF.Functions.ILike(c.Value, $"%{email}%")));
            }
            if (isActive.HasValue)
            {
                patientExpression = patientExpression.And(p => p.IsActive == isActive.Value);
            }

            if (gender.HasValue)
            {
                patientExpression = patientExpression.And(p => p.Gender == gender);

            }

            return patientExpression;
        }
    }
   
}
