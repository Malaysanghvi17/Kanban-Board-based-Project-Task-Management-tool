package com.example.kanban_backend.Model;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.List;

@Entity
public class LaneColumns {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer lid;

    @ManyToOne
    @JoinColumn(name = "bid")
    private Boards board;

    private String title;
    private String label;

    @OneToMany(mappedBy = "lane", cascade = CascadeType.ALL)
    @JsonIgnore
    private List<CardIssues> cards = new ArrayList<>();

    // Getters and setters

    public Integer getLid() {
        return lid;
    }

    public void setLid(Integer lid) {
        this.lid = lid;
    }

    public Boards getBoard() {
        return board;
    }

    public void setBoard(Boards board) {
        this.board = board;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getLabel() {
        return label;
    }

    public void setLabel(String label) {
        this.label = label;
    }

    public List<CardIssues> getCards() {
        return cards;
    }
    @JsonProperty("bid")
    public Integer getBoardId() {
        return board != null ? board.getBid() : null;
    }
    public void setCards(List<CardIssues> cards) {
        this.cards = cards;
    }
}