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
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id,Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest) 
        {
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

       
    }
}
