using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeetupAPI.Data;
using MeetupAPI.DTOs;
using MeetupAPI.Entities;
using MeetupAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IEnumerable<MeetupDTO>> Test()
    {
        return _context.Meetups.ProjectTo<MeetupDTO>(_mapper.ConfigurationProvider);
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
            Name = meetupDto.Name
        };

        await _context.Meetups.AddAsync(meetup);
        await _context.SaveChangesAsync();
        return meetup;
    }
}