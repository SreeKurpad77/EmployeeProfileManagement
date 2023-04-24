using EmployeeProfileManagement.Core.Model;
using EmployeeProfileManagement.Core.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace EmployeeProfileManagement.Core.Repositories
{
    public class EmployeeProfileRepository : IEmployeeProfileRepository
    {
        private readonly IRepository<EmployeeProfile> _repository;
        private readonly ILogger<EmployeeProfileRepository> _logger;
        public EmployeeProfileRepository(IRepository<EmployeeProfile> repository, ILogger<EmployeeProfileRepository> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResultObject<EmployeeProfile>> Add(EmployeeProfile entity)
        {
            ResultObject<EmployeeProfile> result = null;
            try
            {
                var newProfile = await _repository.AddAsync(entity);
                if (newProfile != null)
                    result = CreateResponse<EmployeeProfile>(true, string.Empty, newProfile);
                else
                    result = CreateResponse<EmployeeProfile>(false, Constants.NewProfileCreationErrorMessage, null);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Constants.NewProfileCreationErrorMessage} with an exception {ex.Message}");
                return CreateResponse<EmployeeProfile>(false, Constants.NewProfileCreationErrorMessage, null);
            }
        }

        public async Task<ResultObject<bool>> Delete(int id)
        {
            try
            {
                var isSuccess = await _repository.DeleteAsync(id);
                return CreateResponse<bool>(isSuccess, !isSuccess ? Constants.ExistingProfileDeletionErrorMessage : string.Empty, isSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Constants.ExistingProfileDeletionErrorMessage} with an exception {ex.Message}");
                return CreateResponse<bool>(false, Constants.ExistingProfileDeletionErrorMessage, false);
            }
        }

        public async Task<ResultObject<IEnumerable<EmployeeProfile>>> GetAll()
        {
            try { 
                var profiles = await _repository.GetAllAsync();
                if (profiles != null)
                    return CreateResponse<IEnumerable<EmployeeProfile>>(true, string.Empty, profiles);
                else
                    return CreateResponse<IEnumerable<EmployeeProfile>>(false, Constants.ProfilesRetrievalErrorMessage, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Constants.ExistingProfileUpdationErrorMessage} with an exception {ex.Message}");
                return CreateResponse<IEnumerable<EmployeeProfile>>(false, Constants.ProfilesRetrievalErrorMessage, null);
            }
        }

        public async Task<ResultObject<EmployeeProfile>> GetById(int id)
        {
            try
            {
                var profile = await _repository.GetByIdAsync(id);
                if (profile != null)
                    return CreateResponse<EmployeeProfile>(true, string.Empty, profile);
                else
                    return CreateResponse<EmployeeProfile>(true, Constants.ProfilesNotFoundErrorMessage, null);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{Constants.ProfilesRetrievalErrorMessage} with an exception {ex.Message}");
                return CreateResponse<EmployeeProfile>(false, Constants.ProfilesRetrievalErrorMessage, null);
            }
        }

        public async Task<ResultObject<EmployeeProfile>> Update(EmployeeProfile entity)
        {
            ResultObject<EmployeeProfile> result = null;
            try
            {
                var updatedProfile = await _repository.UpdateAsync(entity);
                if (updatedProfile != null)
                    result = CreateResponse<EmployeeProfile>(true, string.Empty, updatedProfile);
                else
                    result = CreateResponse<EmployeeProfile>(false, Constants.ExistingProfileUpdationErrorMessage, null);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{Constants.ExistingProfileUpdationErrorMessage} with an exception {ex.Message}");
                return CreateResponse<EmployeeProfile>(false, Constants.ExistingProfileUpdationErrorMessage, null);
            }
        }
        private ResultObject<T> CreateResponse<T>( bool isSuccess, string errorMessage, T entity)
        {
            return new ResultObject<T>{ IsSuccess = isSuccess, ErrorMessage = errorMessage, Result = entity };
        }
    }
}