using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Walks.API.Models;
using Walks.API.Repositories;

namespace Walks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository,
            IMapper mapper,IRegionRepository regionRepository,
            IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        [HttpGet]   
        public async Task<IActionResult> GetAllWalksAsync() 
        {
          //Fetch data from database - domain walks

          var walks = await walkRepository.GetAllAsync();

          //Create domain walks to DTO Walks

          var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

          //Return response
          return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id) 
        {
            //Get Walk Domain from database
            var walk = await walkRepository.GetAsync(id);

            //Convert Domain object to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            //return response
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync(
            [FromBody] Models.DTO.AddWalkRequest addWalkRequest) 
        {
            //Validate the incoming request
            if (!(await ValidateAddWalkAsync(addWalkRequest))) 
            {
                return BadRequest(ModelState);
            }
            //Convert DTO to Domain Object
            var walk = new Models.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            //Pass Domain object to repository to persist this
            await walkRepository.AddAsync(walk);

            //Convert Domain object back to DTO 
            var walkDTO = new Models.DTO.Walk
            {
                Id = walk.Id,
                Length = walk.Length,
                Name = walk.Name,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId

            };
            //Send DTO response back to Client 
            return CreatedAtAction(nameof(GetAllWalksAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,
        [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest ) 
        {
            //Validate the incoming request
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest))) 
            {
                return BadRequest(ModelState);
            }

            //Convert DTO to Daomain object 
            var walk = new Models.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId

            };
            //Pass details to Repository - Get Domain object in Domain (or null)
            walk = await walkRepository.UpdateAsync(id,walk);
            //Handle Null (not found) 
            if (walk == null)
            {
              return NotFound();    
            }
            else 
            {
                //Convert back to DTO
                var walkDTO = new Models.DTO.Walk
                {
                    Id = walk.Id,
                    Length = walk.Length,
                    Name = walk.Name,
                    RegionId = walk.RegionId,
                    WalkDifficultyId = walk.WalkDifficultyId

                };
               //return response
                return Ok(walkDTO); 
            }
            
           
        
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id) 
        { 
          //call Repository to delete walk
          var walk = await walkRepository.DeleteAsync(id); 
            
          if (walk == null) 
          {
                return NotFound();
          }
          var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        #region Private methods
        private async Task<bool>  ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest) 
        { 
            if (addWalkRequest == null)
            {
               ModelState.AddModelError(nameof(addWalkRequest),
                        $"{nameof(addWalkRequest)} cannot be empty");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(addWalkRequest.Name)) 
            {
               ModelState.AddModelError(nameof(addWalkRequest.Name),
                        $"{nameof(addWalkRequest.Name)} is required");

            }
            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),
                         $"{nameof(addWalkRequest.Length)} should be greater than zero");

            }
            var region = regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null) 
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                           $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }
            var walkDifficulty = await walkDifficultyRepository.GetAsync
                (addWalkRequest.WalkDifficultyId);
           if (walkDifficulty == null) 
           {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                              $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
           }
           if (ModelState.ErrorCount > 0) 
           { 
             return false;  
           }
          return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest),
                         $"{nameof(updateWalkRequest)} cannot be empty");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name),
                         $"{nameof(updateWalkRequest.Name)} is required");

            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length),
                         $"{nameof(updateWalkRequest.Length)} should be greater than zero");

            }
            var region = regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                           $"{nameof(updateWalkRequest.RegionId)} is invalid.");
            }
            var walkDifficulty = await walkDifficultyRepository.GetAsync
                (updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                              $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid.");
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
