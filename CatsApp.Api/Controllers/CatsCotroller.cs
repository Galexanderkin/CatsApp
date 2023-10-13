using AutoMapper;
using CatsApp.Api.Models;
using CatsApp.Application.Commands;
using CatsApp.Application.Queries;
using CatsApp.Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatsApp.Api.Controllers;

public class CatsCotroller : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private IValidator<CatModel> _validator;

    public CatsCotroller(IMapper mapper, IMediator mediator, IValidator<CatModel> validator)
    {
        _mapper = mapper;
        _mediator = mediator;
        _validator = validator;
    }

    [HttpGet("{id}")]
    public async Task<CatModel> GetAsync(int id)
    {
        var cat = await _mediator.Send(new GetCatQuery() { Id = id });
        var catModel = _mapper.Map<CatModel>(cat);

        return catModel;
    }

    [HttpGet("")]
    public async Task<Page<CatModel>> GetPageAsync(SearchModel searchModel)
    {       
        var page = await _mediator.Send(new GetCatPageQuery() { SearchText = searchModel.SearchText, PageNum = searchModel.PageNum, PageSize = searchModel.PageSize });
        Page<CatModel> resultPage = new()
        {
            IsLast = page.IsLast,
            Content = page.Content.Select(_mapper.Map<CatModel>)
        };

        return resultPage;
    }

    [HttpPost("")]
    public async Task<int> CreateAsync([FromBody] CatModel catModel)
    {
        return await _mediator.Send(new CreateCatCommand() { Name = catModel.Name, Weight = catModel.Weight, Age = catModel.Age });
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync(int id, [FromBody] CatModel catModel)
    {
        await _mediator.Send(new UpdateCatCommand() { Id = id, Name = catModel.Name, Weight = catModel.Weight, Age = catModel.Age });
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        await _mediator.Send(new DeleteCatCommand() { Id = id});
    }
}
