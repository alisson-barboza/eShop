using AutoMapper;
using eShop.Catalogo.Application.Dtos;
using eShop.Catalogo.Domain;

namespace eShop.Catalogo.Application.AutoMapper
{
    public class CategoriaMapper : Profile
    {
        public CategoriaMapper()
        {
            CreateMap<CategoriaDto, Categoria>()
                    .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));

            CreateMap<Categoria, CategoriaDto>();
        }
    }
}