package com.purlieus.purlieus.adapters;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.support.v4.app.FragmentActivity;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.models.BD_Donate;
import com.purlieus.purlieus.models.BD_Seek;

import java.util.List;

/**
 * Created by Shaurya on 06-Oct-16.
 */

public class DonorAdapter extends RecyclerView.Adapter<DonorAdapter.UserViewHolder> {

private List<BD_Seek> list;
private LayoutInflater inflater;
private Context context;


public DonorAdapter(Context context, List<BD_Seek> list) {
        inflater = LayoutInflater.from(context);
        this.list = list;
        this.context = context;
        }

    @Override
public UserViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        View view = inflater.inflate(R.layout.item_seeker, parent, false);

        UserViewHolder holder = new UserViewHolder(view);

        return holder;
        }

@Override
public void onBindViewHolder(UserViewHolder holder, int position) {

        BD_Seek model = list.get(position);
        holder.name.setText(model.getName());
        holder.bloodGroup.setText(model.getBloodGroup());
        holder.age.setText(Integer.toString(model.getAge()));
        holder.phoneNum.setText(model.getContactNumber());
        holder.sex.setText(model.getSex());

        }

@Override
public int getItemCount() {
        return list.size();
        }

class UserViewHolder extends RecyclerView.ViewHolder implements View.OnClickListener {

    TextView name, bloodGroup, phoneNum, sex, age;
    ImageView callButton;


    public UserViewHolder(View itemView) {
        super(itemView);

        name = (TextView) itemView.findViewById(R.id.donor_name_item);
        bloodGroup = (TextView) itemView.findViewById(R.id.donor_blood_group_item);
        phoneNum = (TextView) itemView.findViewById(R.id.donor_contact_item);
        sex = (TextView) itemView.findViewById(R.id.donor_sex_item);
        age = (TextView) itemView.findViewById(R.id.donor_age_item);

        callButton = (ImageView) itemView.findViewById(R.id.donor_call_button);
        callButton.setOnClickListener(this);
    }


    @Override
    public void onClick(View view) {
        String callUri;
        callUri = "tel:" + phoneNum.getText().toString();

        Intent callIntent = new Intent(Intent.ACTION_CALL);
        callIntent.setData(Uri.parse(callUri));

        try {
            context.startActivity(callIntent);
        }catch (SecurityException ex)
        {
            Toast.makeText(context, "Purlieus does not have permission to call", Toast.LENGTH_LONG);
        }
    }
}
}
