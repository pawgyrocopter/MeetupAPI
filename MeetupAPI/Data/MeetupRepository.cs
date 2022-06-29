using AutoMapper;
using AutoMapper.QueryableExtensions;
using MeetupAPI.DTOs;
using MeetupAPI.Entities;

namespace MeetupAPI.Data;

public class MeetupRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MeetupRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IQueryable<Meetup>> GetAllMeetups()
    {
        return _context.Meetups.AsQueryable();
    }
}