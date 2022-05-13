using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPImpacta.Interface;
using ToDoAPImpacta.Models;

namespace ToDoAPImpacta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJobFactory _jobFactory;

        public JobsController(aspnetWebApplication253bc9b9d9d6a45d484292a2761773502Context context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IJobFactory jobFactory)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _jobFactory = jobFactory;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<Job>> PostJobAsync([FromBody]string teste)
        {
            var userName = User.Identity.Name;
            var user = _userManager.FindByNameAsync(userName);
            Job job = _jobFactory.buildJob(teste, _context, user.GetAwaiter().GetResult().Id);

            if(job != null)
            {
                return Ok(job);
            }

            return NotFound("Erro ao cadastrar o job");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<Job>>> GetJobListAsync()
        {
            var userName = User.Identity.Name;
            var user = _userManager.FindByNameAsync(userName);
            List<Job> jobList = _jobFactory.MountJobList(user.GetAwaiter().GetResult().Id, _context);
            return Ok(jobList);

        }

        [Route("api/[controller]/Finalize")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<ActionResult<Job>> FinalizeJob([FromBody]string id)
        {
            Job finallizedJob = _jobFactory.FinalizeJob(id, _context);
            return Ok(finallizedJob);

        }

        private bool JobExists(string id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
