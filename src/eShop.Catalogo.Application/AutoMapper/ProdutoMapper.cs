using AutoMapper;
using eShop.Catalogo.Application.Dtos;
using eShop.Catalogo.Domain;

namespace eShop.Catalogo.Application.AutoMapper
{
    public class ProdutoMapper : Profile
    {
        public ProdutoMapper()
        {
            CreateMap<Produto, ProdutoDto>()
                    .ForMember(d => d.Largura, o => o.MapFrom(s => s.Dimensoes.Largura))
                    .ForMember(d => d.Altura, o => o.MapFrom(s => s.Dimensoes.Altura))
                    .ForMember(d => d.Profundidade, o => o.MapFrom(s => s.Dimensoes.Profundidade));

            CreateMap<ProdutoDto, Produto>()
                   .ConstructUsing(p =>
                       new Produto(p.Nome, p.Descricao, p.Ativo,
                           p.Valor, p.CategoriaId, p.DataCadastro,
                           p.Imagem, p.Altura, p.Largura, p.Profundidade));
        }
    }
}