using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeetupAPI.Data;
using MeetupAPI.DTOs;
using MeetupAPI.Entities;
using MeetupAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Controllers;

public class MeetupController : BaseApiController
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MeetupController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<MeetupDTO>> GetAllMeetups([FromQuery] string search = "1", //search everything
                                                            [FromQuery] string filter = "1", //filter by keywords/author
                                                            [FromQuery] string sort = "name") //sort by name, date,
    {
        return _context.Meetups.Include(x => x.UsersRegistred).ProjectTo<MeetupDTO>(_mapper.ConfigurationProvider);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MeetupDTO>> GetMeetupById(int id)
    {
        if (id > _context.Meetups.Count()) return NoContent();
        return _mapper.Map<MeetupDTO>(_context.Meetups.FirstOrDefault(x => x.Id == id));
    }

    [HttpPost]
    public async Task<Meetup> CreateMeetup([FromBody] MeetupDTO meetupDto)
    {
        var meetup = new Meetup()
        {
            Name = meetupDto.Name,
            CreationDate = DateTime.Now,
            UsersRegistred = new List<User>()
        };
        await _context.Meetups.AddAsync(meetup);
        await _context.SaveChangesAsync();
        return meetup;
    }

    [HttpPost("{id}/register", Name = "register")]
    public async Task<MeetupDTO> RegisterForMeetup(int id)
    {
        var meetup = _context.Meetups.Include(x => x.UsersRegistred).FirstOrDefault(x => x.Id == id);
        var user = _context.Users.FirstOrDefault(x => x.UserName.Equals(HttpContext.User.Identity.Name));
        meetup.UsersRegistred.Add(user);
        await _context.SaveChangesAsync();
        return _mapper.Map<MeetupDTO>(meetup);
    }
}