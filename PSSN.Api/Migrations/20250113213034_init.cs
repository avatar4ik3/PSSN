using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PSSN.Api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "scheduled_research");

            migrationBuilder.CreateTable(
                name: "GenerationResults",
                schema: "scheduled_research",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_generation_results", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "Research",
                schema: "scheduled_research",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    strategies_count = table.Column<int>(type: "integer", nullable: false),
                    distribution_step = table.Column<decimal>(type: "numeric", nullable: false),
                    ro = table.Column<List<double>>(type: "double precision[]", nullable: false),
                    count_of_experiments = table.Column<int>(type: "integer", nullable: false),
                    gen_count = table.Column<int>(type: "integer", nullable: false),
                    swap_chance = table.Column<double>(type: "double precision", nullable: false),
                    crossing_count = table.Column<int>(type: "integer", nullable: false),
                    selection_group_size = table.Column<int>(type: "integer", nullable: false),
                    count_of_distributions = table.Column<int>(type: "integer", nullable: false),
                    total_games_count = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_research", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "ConditionalStrategy",
                schema: "scheduled_research",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    pattern_name = table.Column<string>(type: "text", nullable: false),
                    pattern_coefs = table.Column<int[]>(type: "integer[]", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false),
                    generation_results_guid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conditional_strategy", x => x.guid);
                    table.ForeignKey(
                        name: "fk_conditional_strategy_generation_results_generation_results_gu",
                        column: x => x.generation_results_guid,
                        principalSchema: "scheduled_research",
                        principalTable: "GenerationResults",
                        principalColumn: "guid");
                });

            migrationBuilder.CreateTable(
                name: "GameResults",
                schema: "scheduled_research",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    current_experiment = table.Column<int>(type: "integer", nullable: false),
                    current_distribution = table.Column<decimal>(type: "numeric", nullable: false),
                    current_generation = table.Column<int>(type: "integer", nullable: false),
                    generation_results_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    research_guid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_results", x => x.guid);
                    table.ForeignKey(
                        name: "fk_game_results_generation_results_generation_results_guid",
                        column: x => x.generation_results_guid,
                        principalSchema: "scheduled_research",
                        principalTable: "GenerationResults",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_results_research_research_guid",
                        column: x => x.research_guid,
                        principalSchema: "scheduled_research",
                        principalTable: "Research",
                        principalColumn: "guid");
                });

            migrationBuilder.CreateTable(
                name: "GenerationTreeNode",
                schema: "scheduled_research",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    strategy1guid = table.Column<Guid>(type: "uuid", nullable: false),
                    strategy2guid = table.Column<Guid>(type: "uuid", nullable: false),
                    results = table.Column<List<double>>(type: "double precision[]", nullable: false),
                    generation_results_guid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_generation_tree_node", x => x.guid);
                    table.ForeignKey(
                        name: "fk_generation_tree_node_conditional_strategy_strategy1guid",
                        column: x => x.strategy1guid,
                        principalSchema: "scheduled_research",
                        principalTable: "ConditionalStrategy",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_generation_tree_node_conditional_strategy_strategy2guid",
                        column: x => x.strategy2guid,
                        principalSchema: "scheduled_research",
                        principalTable: "ConditionalStrategy",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_generation_tree_node_generation_results_generation_results_guid",
                        column: x => x.generation_results_guid,
                        principalSchema: "scheduled_research",
                        principalTable: "GenerationResults",
                        principalColumn: "guid");
                });

            migrationBuilder.CreateIndex(
                name: "ix_conditional_strategy_generation_results_guid",
                schema: "scheduled_research",
                table: "ConditionalStrategy",
                column: "generation_results_guid");

            migrationBuilder.CreateIndex(
                name: "ix_game_results_generation_results_guid",
                schema: "scheduled_research",
                table: "GameResults",
                column: "generation_results_guid");

            migrationBuilder.CreateIndex(
                name: "ix_game_results_research_guid",
                schema: "scheduled_research",
                table: "GameResults",
                column: "research_guid");

            migrationBuilder.CreateIndex(
                name: "ix_generation_tree_node_generation_results_guid",
                schema: "scheduled_research",
                table: "GenerationTreeNode",
                column: "generation_results_guid");

            migrationBuilder.CreateIndex(
                name: "ix_generation_tree_node_strategy1guid",
                schema: "scheduled_research",
                table: "GenerationTreeNode",
                column: "strategy1guid");

            migrationBuilder.CreateIndex(
                name: "ix_generation_tree_node_strategy2guid",
                schema: "scheduled_research",
                table: "GenerationTreeNode",
                column: "strategy2guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameResults",
                schema: "scheduled_research");

            migrationBuilder.DropTable(
                name: "GenerationTreeNode",
                schema: "scheduled_research");

            migrationBuilder.DropTable(
                name: "Research",
                schema: "scheduled_research");

            migrationBuilder.DropTable(
                name: "ConditionalStrategy",
                schema: "scheduled_research");

            migrationBuilder.DropTable(
                name: "GenerationResults",
                schema: "scheduled_research");
        }
    }
}
