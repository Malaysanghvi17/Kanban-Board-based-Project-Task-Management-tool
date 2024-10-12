package com.example.kanban_backend.Repository;
import com.example.kanban_backend.Model.CardIssues;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
@Repository
public interface CardIssuesRepository extends JpaRepository<CardIssues, Integer> {
    List<CardIssues> findByLaneLid(Integer lid);
}