using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Walks.API.Repositories;

namespace Walks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository,
            IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficulties = await walkDifficultyRepository.GetAllAsync();
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);
            return Ok(walkDifficultiesDTO);

        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }
            //Convert Domain to DTO

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync
            (Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Validate incoming request
            if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest)) 
            {
                return BadRequest(ModelState);
            }
            //Convert DTO to Domain Model
            var walkDifficulty = new Models.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code,
            };
            // Call repository
            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            //Convert Domain To DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            //return response
            return CreatedAtAction(nameof(GetWalkDifficultyById),
                new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Validate incoming request
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest)) 
            { 
                return BadRequest(ModelState);    
            
            }
            //Convert DTO to Domain Model
            var walkDifficulty = new Models.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code,
            };

            //Call repository
            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);
            if (walkDifficulty == null)
            {
                return NotFound();
            }
            //Convert Domain to DTO 
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            // return response
            return Ok(walkDifficultyDTO);


        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }
            //Convert to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }
        #region Private methods
        private bool ValidateAddWalkDifficultyAsync(
          Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        { 
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest),
                     $"{nameof(addWalkDifficultyRequest)} is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code)) 
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                       $"{nameof(addWalkDifficultyRequest.Code)} is required.");

            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        private bool ValidateUpdateWalkDifficultyAsync(
          Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                     $"{nameof(updateWalkDifficultyRequest)} is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                       $"{nameof(updateWalkDifficultyRequest.Code)} is required.");

            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
