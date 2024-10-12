package com.example.kanban_backend.Model;

import com.fasterxml.jackson.annotation.JsonBackReference;
import jakarta.persistence.*;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Entity
public class Boards {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer bid;

    @ManyToOne
    @JoinColumn(name = "uid")
    @JsonBackReference
    private Users owner;

    private String name;

    @OneToMany(mappedBy = "board", cascade = CascadeType.ALL)
    private List<LaneColumns> lanes = new ArrayList<>();

    // Getters and setters

    public Integer getBid() {
        return bid;
    }

    public void setBid(Integer bid) {
        this.bid = bid;
    }

    public Users getOwner() {
        return owner;
    }

    public void setOwner(Users owner) {
        this.owner = owner;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public List<LaneColumns> getLanes() {
        return lanes;
    }

    public void setLanes(List<LaneColumns> lanes) {
        this.lanes = lanes;
    }
}