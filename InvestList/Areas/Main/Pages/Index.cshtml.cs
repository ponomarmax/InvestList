using System.Globalization;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using InvestList.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Radar.Application.Models;

namespace InvestList.Areas.Main.Pages
{
    public class Index( IMediator mediator) : PageModel
    {
        public IEnumerable<PostShortDto> BlacklistPosts { get; set; }
        public IEnumerable<PostShortWithCommentDto> PostsWithLastComments { get; set; }
        public IEnumerable<PostShortDto> NewsPosts { get; set; }
        public IEnumerable<InvestPostShortDto> InvestPosts { get; set; }
        public List<Guid> TagIds { get; set; }
        public string Search { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1,
            List<Guid> tagIds = null,
            string search = null)

        {
            Search = search;
            if (pageIndex < 1)
            {
                return NotFound();
            }
            
            var command = new GetPrimarySectionsQuery()
            {
                Search = search,
                TagIds = tagIds
            };

            var result = await mediator.Send(command);

            NewsPosts = result.News;
            BlacklistPosts = result.Blacklists;
            InvestPosts = result.Invests?.Take(3);
            PostsWithLastComments = result.PostWithComments;

            TagIds = tagIds;

            return Page();
        }
    }
}